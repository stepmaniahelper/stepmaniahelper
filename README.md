# Welcome to StepManiaHelper
*A song library management tool for StepMania.*

## General
The original idea of the tool was that I had a song library of over 10k songs, and that caused the game to take a long time to startup. Since many of these songs were ones I'd never play (some were too difficult for me, some were slow and boring, and others were made for keyboards while I play with a dance pad), it seemed kind of silly to have them in my song library slowing down my load time. However, separating out these songs was a task I felt was better automated than done manually. 

Thanks to some user-suggested changes, the tool has evolved a bit towards becoming a more general song library management tool, although it's still a far-cry from fully featured, and it is still very much held back by its ad-hoc design and development.

![Screenshot](https://i.imgur.com/367FbHr.png)

## Dependencies
If you are running Windows 10+, you shouldn't need do download anything other than the application itself, but if you're running on an older OS you may have to download the .Net Framework 4.5 from Microsoft: https://www.microsoft.com/en-us/download/details.aspx?id=30653

## Getting Started
There's a lot of functionality available, so I'll try to break down the typical workflow after running for the first time:

- Point the top most textbox to your StepMania "Songs" directory. This can by done by typing the path in, or using the "..." button to user Windows folder selection tool.

- By default, pressing the "Search/Parse/Run-Special-Filter" button does nothing. To make it do something, you need to select some options from the "Program Options" menu on the left. I would recommend selecting the following options:

  - "Search for new songs"

    - This will cause the program to scan the selected folder for StepMania files.

  - "Parse only unparsed songs"

    - After identifying all the relevant folders and files, it will parse them for information about the song.

  - "Load saved data (if available)"

    - Won't do anything the first time you run the program, but if you also select "Save data after parse", it will prevent you having to search and parse for songs if you re-run the application

  - "Save data after parse"

    - Will save a "StepManiaHelper.json" in the directory you point the program to. This file contains information gathered from the song files and is used to speed things up on subsequent runs of the application.

- Press the "Search/Parse/Run-Special-Filter" button to have it identify and parse your song library. It will first find all the songs, and then parse them. The "Songs List" will fill with songs as they are found, and progress will be shown in the "Parse/Filter Output" section.

- By default, only 3 headings are shown in the "Song List". These can be added or removed using the "List Headers" dropdown and "Header Visible" checkbox next to it. Columns can also be rearranged by dragging their headers within the grid. Columns cannot be sorted while an operation is in progress, but after the searching and parsing is complete, you can sort them.

## Next Steps
Once your song library has been parsed, there's 3 things you might want to do:

- Export your song list to a CSV file using the "Export" button. This will allow you to view your library in other tools like excel.

- Create custom song packs. This will copy songs to a new song pack folder, allowing you to create things like "Favorites".

- Create a search and filter combo to hide songs from StepMania (but not delete them). The search is fairly configurable but I usually use it to hide songs that are too difficult or fast for me to play.

## Song Search
Song searches are ways to find specific songs within your song list. To start creating a search, click the "Add Search Operand" button.

Each search operand is linked to a property of a song (one of the available headers in the song list), and this property can be changed using the leftmost dropdown. There are two types of properties, numeric and strings (text), and switching between properties of these different types will change the style of the operand row itself.

For string properties, the matching is performed using regular expression (regex), but as long as you only use numbers and letters, you can treat it as a simple text search. This allows you to search for songs that have the word "hello" in them for instance.

For numeric properties, you need to select both a comparator and a value. A comparator is something like "equal to" or "greater than". Using for example the "LowDifficulty" property (which is the lowest numerical difficulty associated with the song) and the "less than" comparator, we can create a search to find all song with a lowest difficult of below 5.

You can create as many operands as you need for a search, and they are combined using one of two options: "AND (all must be true)" and "OR (any can be true)".

To run a defined search, click the "Apply search" button which replace the songs in the song list with only songs that match. Conversely, clicking "Clear search" will restore the full list of songs.

If you desire to create multiple searches, you can click the "Search Name" combo-box and replace the "Add New" text with the name of the search you want to create. This will add the entered text as an option in the combo-box, and save all the operands with that name. To define a new search, select the "Add New" option from the combo-box, and modify the operands for the new search. Selecting a search from the dropdown will change the operands in the area below the dropdown to match what was previously saved.

## Filter Folders / Custom Song Pack Folders
There are two types of folders that the program supports:

- Filters

  - These will move songs outside the StepMania "Songs" directory so that the games doesn't see them anymore.

- Custom Song Pack

  - Folder inside the "Songs" directory that will look like just another song pack.

To define a folder, click inside the "Folder Name" combo-box and replace the "Add New" text with the name of the folder you want to create. Once you've named a folder, click the radio-buttons below it to choose whether the folder will be a custom-song-pack or a filter.

Once a folder has been defined, it will appear as an option in the song list headers dropdown. Making this column visible will show a checkbox for each song, allowing you to apply the folder to that song by checking the box, or un-associating the folder by unchecking it.

The "Apply to all visible" and "Clear from all visible" buttons will effectively check or uncheck the box of every song visible in the song list for the folder that is currently selected. On it's own that's not very helpful (I don't think most people want to hide all of their songs, or put all their songs into a single custom song pack), but combined with the song search area beneath the folder area, this can be used to quickly hide songs from StepMania that meet a specific criteria.

If a global hotkey is defined, this hotkey can be pressed to toggle the filter/custom-song-pack for the currently selected song in the song list. Note that this hotkey is global, meaning that even if the application is minimized or not focused, it will still take effect (see the Game Monitor section for why this is useful.)

## Special Filters

The application comes with 2 special filters which are similar to the customizable ones, but utilize a fixed filter folder name. If selected, these filters are applied during the "Search/Parse/Run-Special-Filter" operation.

- Duplicates
	
  - The duplicate filter will only match songs if they have identical step-charts. If two (or more) songs with identical step charts are found during the scan, only one of these will be left in the "Songs" directory, and all others will be moved to the "_DUPLICATE" filter folder.

- Alternates

  - The alternates filter allows for identification of songs that are similar but not identical. A similarity threshold is specified, and songs that meet or exceed this threshold will be matched. Song titles, artists, music files, BPM, and beat count are all considered in this evaluation. Identified alternate versions of songs will be moved to the "_ALT" filter folder.

## Game Monitor

Song management from a dedicated app can be useful sometimes, but sometimes you don't know that you like (or hate) a particular song unless you just finished playing it; if you then want to filter or save this song using the app, you have to first find that same song in the app, which can be tedious. Enter the game monitor. 

The game monitor will ask the operating system to tell it about any files that the game executable interacts with, and it can compare these files against the list of songs it found during previously run scans, allowing it to determine which sing is currently selected in the game. In practive, this means that when the game selects a song, if the game monitor is active, it will select this same song in the app. This functionality can be combined with the global hotkeys defined as part of a filter/custom-song-pack to manage your song library all from within the game.

Please note that a filter which removes the song from the games "Songs" directory cannot be applied while the game has its associated files open. If the application detects this happening, it will queue the filter, and it will be applied when the game's song selection next changes.

Please note that the game monitor functionality will only work if the application is run with administrator priveleges.

## Saving
All changes made by the user (visibility and order of columns, checkbox states, folders and song searches, etc.) are saved in a "SavedOptions.json" beside the executable when the application closes. So you should only have to configure the program to your liking once, and it will save that info for the next run.

## Conclusion
This is an alpha build, so I expect there to be bugs and usability issues still. If you want to give the program a try, please let me know if you run into issues with a feature. Also, if you would like additional functionality to be added, please let me know and I'll see what I can do :p
