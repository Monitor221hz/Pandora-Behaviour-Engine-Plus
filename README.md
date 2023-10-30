# Pandora Behaviour Engine+

A modular and lightweight behavior engine for TES Skyrim SE, for creatures and humanoids.  

Built with backwards compatibility in mind for [Nemesis Unlimited Behavior Engine](https://github.com/ShikyoKira/Project-New-Reign---Nemesis-Main) and designed to be an alternative, Pandora streamlines both the author and user experience through a simplified UI, robust logging, intuitive formats, and fast patching times.
<br/>
<br/>

## Navigation
* [For Users](#for-users)
  * [Quickstart](#quickstart)
* [For Mod Authors](#for-mod-authors)
  * [File Targeting](#file-targeting)
    * [Unique Identifiers](#unique-identifiers)
    * [Indirect Identifiers](#indirect-identifiers)
  * [AnimData](#animdata)
    * [Auto Generation](#auto-generation)
    * [Manual Addition](#manual-addition)
  * [AnimSetData](#animsetdata)
    * [Adding AnimSetData Animations](#adding-animsetdata-animations)
  * [Custom Projects](#custom-projects)
  * [Verbose Logging](#verbose-logging)
    * [Severity](#severity)
    * [Component](#component)
    * [Input](#input)

<br/>
<br/>

## For Users

### Quickstart
Install [.NET 7 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) if you do not have it installed.  

<br/>

<details>

<summary>MO2 Users</summary>
<br/>

1. Install Pandora Behaviour Engine as a mod and make sure it is active.
2. Add Pandora as an [application for MO2](https://stepmodifications.org/wiki/Guide:Mod_Organizer#General_Application_Setup)
    * Having a dedicated output mod is recommended to keep the files that the tool generates in one place.
3. Run the program from within MO2, using the menu on the right.
4. Tick the patches you want and click Launch. 


</details>
<br/>
<details>

<summary>Vortex Users</summary>
<br/>

1. Install Pandora Behaviour Engine as a mod and make sure it is active.
2. Run the program either manually or after adding it to the tools dashboard.
3. Tick the patches you want and click Launch. 

</details>

<br/>



In case of any problems with the output, `Engine.log` names patches with failed edits for easier troubleshooting. 

It's recommended to pass the log on to the relevant mod author if a specific mod seems to show up in the warnings too many times, as it could be an error with the patch itself.
<br/>
<br/>
## For Mod Authors
This section exists to inform current behavior authors of the key differences and features of Pandora, it's not a guide for making behavior mods.
### File Targeting

#### Unique Identifiers
Pandora supports the same [patch file format](https://github.com/ShikyoKira/Project-New-Reign---Nemesis-Sub-tool) as Nemesis, but the folder system is expanded on to provide creature compatibility.  Patch folders can use the short name or their full unique name to be recognized by the engine.  
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

#### Indirect Identifiers
Skeleton and character files can be targeted using either short or full names. Alternatively they can also use:
<br/>
<br/>
* `[ProjectName]_character` for targeting the character file of the project.

<br/>

* `[ProjectName]_skeleton` for targeting the skeleton file of the project.
<br/>

### AnimData
#### Auto Generation
Pandora Behavior Engine generates dummy motion data for every added clip generator. All added animations are now [AMR ready](https://www.nexusmods.com/skyrimspecialedition/mods/50258) by default when patched. 
<br/>

No more need for blank animdata to be written by hand, which was a huge hassle for behavior authors.  
<br/>

#### Manual Addition
Mods with their own custom graphs still need to register their clip generators manually if they want motion.  

<br/>

To do so, create a folder named `animdata` and create a `[ProjectName].txt` file with each line containing the name of one clip generator. Don't worry about repetition as the engine will automatically discard duplicate clip generator names (case sensitive).  

There's also a [python script](https://gist.github.com/Monitor144hz/ce3069fb99064bda85e9b127f90e5039) that automatically converts the Nemesis animdatasinglefile format into the Pandora format, for when there are too many clip generator names to convert manually.

<br/>
<br/>

### AnimSetData
AnimSetData is a unique case that Pandora does not automatically generate, because it's not necessary for all new animations and would be a waste of time to find and automatically generate for every new animation file. 
<br/>

It's only needed for adding paired animations and other edge cases this section can be skipped if it's not relevant.
<br/>
Currently Pandora only supports adding anim info, for paired animations. 
<br/>

#### Adding AnimSetData Animations
To make additions to AnimSetData, authors must define animation paths in a separate file with this folder structure.
<br/>
```
[ModFolder]\animsetdatasinglefile\[ProjectName]\[SetName].txt
```
<br/>
<br/>

Each file of the project folder should have paths with the animation relative to the data folder. For example, in path:
```
..\testmod\animsetdatasinglefile\DefaultMale\H2HDual.txt
```

<br/>
There would be something like:
<br/>

```
meshes\actors\character\animations\killmove1.hkx
meshes\actors\character\animations\killmove2.hkx
meshes\actors\character\animations\killmove3.hkx
```

These animations are then parsed, encoded with the right format, and added to animsetdata.
<br/>

Formerly, authors would have to encode the file and paths themselves and add it to the copied set file between two comments indicating the operation. Now it's as easy as writing a single line in a new file with the same name!


<br/>
<br/>

### Custom Projects

Not yet implemented.

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
* `FATAL` means the engine has failed completely because of a significant fault, usually on export. These should be reported immediately to the engine developer(s).

#### Component:

* The `Assembler` is responsible for parsing the patch format to derive an operation for each edit. Usually the name is preceded by the format that it parses.
  * The assembler responsible for parsing Nemesis patches is the `Nemesis Assembler`.
* The `Dispatcher` is responsible for saving the patch edits and applying them to the target file.
* The `Validator` is responsible for validating all the edits made by the Dispatcher after it is run.

#### Input:

The input is usually a parameter of the operation that is significant for manual debugging. The most common input are xml paths which denote the path to the area of the xml file where an edit failed.
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
