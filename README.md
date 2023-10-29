# Pandora Plus Behavior Engine

A modular and lightweight behavior engine for TES Skyrim SE, for creatures and humanoids.  

Built with backwards compatibility in mind for [Nemesis Unlimited Behavior Engine](https://github.com/ShikyoKira/Project-New-Reign---Nemesis-Main) and designed to be an alternative, Pandora streamlines the user experience through a simplified 
but comfortable UI, robust logging, and fast patching times.
<br/>
<br/>

## Navigation
* [For Users](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#for-users)
  * [Quickstart](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#quickstart)
* [For Mod Authors](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#for-mod-authors)
  * [File Targeting](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#file-targeting)
  * [AnimData](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#animdata)
  * [AnimSetData](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#animsetdata)
  * [Verbose Logging](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#verbose-logging)
    * [Severity](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#severity)
    * [Component](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#component)
    * [Input](https://github.com/Monitor144hz/Pandora-Plus-Behavior-Engine#input)

<br/>
<br/>

## For Users

### Quickstart
Install [.NET 7 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) if you do not have it installed.  

1. Run the behavior engine. If you're using MO2 you must launch from within the manager.
2. Tick any mods that you want patched.
3. Click "Launch" and let the engine finish.

In case of any problems with the output, `Engine.log` names patches with failed edits for easier troubleshooting. 

It's recommended to pass the log on to the relevant mod author if a specific mod seems to show up in the warnings too many times, as it could be an error with the patch itself.
<br/>
<br/>
## For Mod Authors

### File Targeting
For now, Pandora uses the same patch file format as Nemesis, but the folder system is expanded on to provide creature compatibility.  Patch folders can use the short name or their full unique name to be recognized by the engine.  
<br/>
The full identifying name is `[ProjectName]~[FileName]`. For example:
<br/>

* `0_master` is also recognized as `defaultmale~0_master`.
    * `_1stperson~0_master` targets a different file in the 1st person project.

<br/>

* `horsebehavior` is also recognized as `defaultmale~horsebehavior`
    * `horseproject~horsebehavior` targets a different file in the horse project.
  
<br/>
<br/>
Note that using the full name does not separate files that are already shared between projects, it is only there to resolve naming conflicts.
<br/>
<br/>

Skeleton and character files can be targeted using either short and full names folder names. Alternatively they can also use:
<br/>
<br/>
* `[ProjectName]_character` for targeting the character file of the project.

<br/>

* `[ProjectName]_skeleton` for targeting the skeleton file of the project.
<br/>

### AnimData
Pandora Behavior Engine generates dummy motion data for every added clip generator. All added animations are now [AMR ready](https://www.nexusmods.com/skyrimspecialedition/mods/50258) by default when patched. No more need for blank animdata to be written 
by hand, which was a huge hassle for behavior authors.
<br/>
<br/>

### AnimSetData
Work in progress.
<br/>
<br/>

### Verbose Logging
Log messages are kept in a separate file called `Engine.log`. 

Messages loosely follow this format:
<br/>

`[Severity]: [Component] > [Data] > [Operation] > [Input] > [Status]`

<br/>

Most of these are self explanatory but some of the more obscure formatting is explained below.
<br/>

#### Severity: 

* `INFO` means it's just there as a notification. Nothing has gone wrong.
* `WARN` means something is unexpected and could be a potential issue. 
* `ERROR` means the subject prevented the engine from performing some part of its work and is likely to be an issue.
* `FATAL` means the engine has failed completely because of a significant fault.

#### Component:

* The `Assembler` is responsible for parsing the patch format to derive an operation for each edit. Usually the name is preceded by the format that it parses. The assembler responsible for parsing Nemesis patches is the `Nemesis Assembler`.
* The `Dispatcher` is responsible for saving the patch edits and applying them to the target file.
* The `Validator` is responsible for validating all the edits made by the Dispatcher after it is run.

#### Input:

The input is usually a parameter of the operation that is significant for manual debugging. The most common input are xml paths which denote the path to the area of the xml file where it failed.
<br/>

Can you see why this edit failed?
<br/>

```


Dispatcher > "ExampleMod" > defaultfemale~1hm_behavior > Replace > Element > #2521/event/Element0/id > FAILED
```

```xml
<hkobject name="#2521" class="BSRagdollContactListenerModifier" signature="0x8003d8ce">
  <hkparam name="variableBindingSet">null</hkparam>
  <hkparam name="userData">2</hkparam>
  <hkparam name="name">VictimState_RagdollListener</hkparam>
  <hkparam name="enable">true</hkparam>
  <hkparam name="contactEvent">
    <hkobject>
      <hkparam name="id">74</hkparam>
      <hkparam name="payload">null</hkparam>
    </hkobject>
  </hkparam>
  <hkparam name="bones">#2520</hkparam>
</hkobject>
```

<details>
  <summary>
    Answer:
  </summary>
  
  The path should be `#2521/contactEvent/Element0/id`, not `#2521/event/Element0/id`. In this case, it should be fixed by the author, or reported to the author if found by a user.
</details>
