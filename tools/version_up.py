import re
from subprocess import CalledProcessError, check_output, getoutput
import sys
from pathlib import Path


def usage():
    return """
    This is an upgrade script for `Pandora-Behavior-Engine-Plus` repository owners only.
    It is not intended to be run on forked projects.

    Usage:
    python version_bump.py <version_type>

    Version Types:
    1. major: Increment the major version (e.g. 0.0.0 -> 1.0.0)
    2. minor: Increment the minor version (e.g. 0.0.0 -> 0.1.0)
    3. patch: Increment the patch version (e.g. 0.0.0 -> 0.0.1)

    Example:
    python ./version_bump.py minor
    python ./version_bump.py 2

    Update with default settings.
    python ./version_bump.py
    """


def main():
    # ---------- Your default configurations --------------------------------
    default_ver_type = "3" # 3: Update the patch version.
    new_ver_label = "alpha"

    repo_root = getoutput("git rev-parse --show-toplevel")
    c_sharp_meta_path = (
        f"{repo_root}/PandoraPlus/MVVM/Model/Patch/Patchers/Skyrim/SkyrimPatcher.cs"
    )
    meta_ini_path = f"{repo_root}/PandoraPlus/meta.ini"

    # Git commit configuration
    dry_run= False # No commits or tags are created. For testing.
    use_gpg = False # Give the option to do a signed commit with the gpg key.
    push = False
    # ---------------------------------------------------------------

    version_type = sys.argv[1] if len(sys.argv) >= 2 else default_ver_type
    if not Path(repo_root).exists():
        print(f"Not found repository root path: {repo_root}")
        sys.exit(1)

    # Update C# file meta info
    if not Path(c_sharp_meta_path).exists():
        print(f"File not found: {c_sharp_meta_path}")
        sys.exit(1)
    [current_ver, new_ver, ver_label] = update_c_sharp_ver_file(c_sharp_meta_path, version_type, new_ver_label)

    current_ver = f"{current_ver}-{ver_label}"
    new_ver= f"{new_ver}-{ver_label}"

    # Update meta.ini
    update_version_in_meta_ini(meta_ini_path, new_ver)

    # Git commit & tag
    if not dry_run:
      git_commit_and_tag(current_ver, new_ver, use_gpg, push)

    print(f"Updated version: {current_ver} => {new_ver}")


def bump_version(current_version: str, version_type: str):
    """
    The next version is calculated from the current version given in the argument.

    # Expected string examples
    current_version: "1.4.0"
    version_type: "2"

    # Invalid string examples
    with suffix: "1.4.0-alpha"
    """
    major, minor, patch = map(int, current_version.split('.'))

    if version_type in {'major', '1'}:
        return f"{major + 1}.0.0"
    elif version_type in {'minor', '2'}:
        return f"{major}.{minor + 1}.0"
    elif version_type in {'patch', '3'}:
        return f"{major}.{minor}.{patch + 1}"
    else:
        raise ValueError(f"Invalid version type. Please specify 'major'(1), 'minor'(2), or 'patch'(3).\n{usage()}")


def update_c_sharp_ver_file(file_path: str, version_type: str, ver_label: str):
    """
    Update C# version information file.

    # Returns example
    ["1.4.0", "1.5.0", "alpha"]

    [current, new, suffix(ver_label)]

    # Safety
    When do we have to fix this code?
    - If the version up code has changed.

      Example:
      ```diff
      - `static readonly string versionLabel =`
      + `static string versionLabel =`
      ```
    """

    version_pattern = re.compile(
    r"static readonly Version currentVersion = new Version\((\d+), (\d+), (\d+)\);"
    )
    version_label_pattern = re.compile(r'static readonly string versionLabel = "(.*)";')

    # 1/3 Read C# version info file.
    with open(file_path, "r", encoding="utf_8_sig") as file: # utf_8_sig: UTF-8-BOM
        content = file.read()

    # 2/3 Get current version.
    current_version_match = version_pattern.search(content)
    if not current_version_match:
        raise ValueError("Current version not found in the file.")
    current_version = f"{current_version_match[1]}.{current_version_match[2]}.{current_version_match[3]}"

    # 3/3 Replace current version with new version
    new_version = bump_version(current_version, version_type)
    new_content = version_pattern.sub(
        f"static readonly Version currentVersion = new({new_version.replace(".", ", ")});", content
    )
    new_content = version_label_pattern.sub(
    f'static readonly string versionLabel = "{ver_label}";', new_content
    )

    # NOTE: Use newline = "\n"(`LF`). otherwise couldn't commit.
    with open(file_path, "w", newline="\n") as file:
        file.write(new_content)

    return [current_version, new_version, ver_label]


def update_version_in_meta_ini(meta_ini_path: str, new_version: str):
    with open(meta_ini_path, 'r') as file:
        content = file.read()

    content = re.sub(r'version=\d+\.\d+\.\d+(-\w+)?', f'version={new_version}', content)

    # NOTE: Use newline = "\n"(`LF`). otherwise couldn't commit.
    with open(meta_ini_path, 'w', newline="\n") as file:
        file.write(content)


def git_commit_and_tag(current_version: str, new_version: str, use_gpg: bool = False, push: bool= False):
    """
    Do the following two things
    - Commit files.

    - A tag is created with a v-prefix.
      e.g. new_version: `1.4.0-alpha` -> `v1.4.0-alpha`

    # `use_gpg` option
    Give the option to do a signed commit with the gpg key.(default: False)

    # `push` option
    Push the code to the remote at once.(default: False)
    """
    tag_flags = ""
    commit_flags = ""
    if use_gpg:
        tag_flags += "-s "
        commit_flags += "-S "

    try:
        # Commit changes to Git
        check_output(
            f'git add . && git commit {commit_flags} -m "build(version): bump PandoraPlus from {current_version} to {new_version}"',
            shell=True,
        )

        # Create Git tag
        check_output(
            f'git tag v{new_version} {tag_flags} -m "Version {new_version}"', shell=True
        )

        # Git push(commit & tags)
        if push:
          check_output('git push && git push origin --tags', shell=True)

        print("Git commit and tag created successfully.")
    except CalledProcessError as error:
        raise RuntimeError(f"Failed to create Git commit and tag: {error}") from error


def git_undo():
  """Reset commit & tag"""
  # Please don't forget to remove newline
  prev_tag = check_output("git describe --tags --abbrev=0").rstrip().decode("utf-8")
  check_output(f"git tag -d {prev_tag}") # Delete prev tag
  check_output("git reset --soft HEAD^") # Staging the previous commit.


if __name__ == "__main__":
    main()
