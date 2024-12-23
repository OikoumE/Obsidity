# Obsidity Plugin
## Description
### Obsidian.md + Unity = Obsidity!

A tiny plugin that docks right in UnityEditor that allows you to take (Obsidian.md style markdown) notes easily and quickly.

---


## Installation
1. copy the repo-url (https://github.com/OikoumE/Obsidity.git)
2. open unity package manager
3. click "add from github"
4. paste URL

<img src="https://github.com/user-attachments/assets/813eda99-27b5-43ee-b4d3-679a3076e993" alt="image" width="200" />

The 'Obsidity Welcome' window should appear.
*(you can always find it by going to 'Window>Obsidity>Obsidity Intro')*

When you initialize a Vault, the Intro window will close, and the editor window will appear in its place.


<img src="https://github.com/user-attachments/assets/6856b136-576b-47af-98d9-0165e0ebbf2b" alt="image" width="200" /><img src="https://github.com/user-attachments/assets/2692f86b-b0ce-4e1e-a934-80f544449091" alt="image" width="200" />

After initializing you can access the Obsidity Editor via 'Window>Obsidity>Obsidity Editor'.

If you want to use Obsidian.md to read the notes, simply open Obsidian, in the bottom left corner you have the "Vault Manager", click it and select "Open folder as vault", 
navigate to '<your unity project path>/Assets/Obsidity/<Name of your vault>'. 


<img src="https://github.com/user-attachments/assets/e7483e1a-b9b6-4036-88ef-5839d0e59b41" alt="image" width="200" />

<img src="https://github.com/user-attachments/assets/49db26e1-77e1-4bf5-8b5a-c0e4e24449c2" alt="image" width="200" />


you should see a <.obsidian> folder present, accept and Obsidian.md should automatically open a new window.


<img src="https://github.com/user-attachments/assets/5097430c-7e16-493b-a72b-5742cd26d749" alt="image" width="200" />

---

## Usage
Currently the plugin requires all fields to be filled to save a file.
You can hit Shift+Enter to save, if enabled and the large input field has focus.

When saving with Obsidity, new files are saved to: 'Assets/Obsidity/<VaultName>/**ObsidityNotes**'.
If you pair Obsidian.mb to the vault, new notes created with Obsidian is saved to **ObsidianNotes**.
(the Obsidian save path can be changed via '/Assets/Obsidity/<YourVault>/.obsidian/**app.json**' or via Obsidian settings)

---

## Support
cant promise anything but you can try opening an issue here in the repo

---

## Liscence
OpenSauce

---

### Changelog:
26/11/24:
  - ~~added export_unitypackage.yml workflow~~
  - changed from exporting unitypackage to importing github repo workflow
01/12/24:
  - added (multiple) settings and a few minor changes
  
---
