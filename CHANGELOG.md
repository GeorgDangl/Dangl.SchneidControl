# Changelog

All notable changes to **Dangl.SchneidControl** are documented here.

## v1.0.14:
- Add option to set exchanged heat meters
- The backend was updated to .NET 9

## v1.0.13:
- The frontend was updated to Angular v18
- Backend dependencies were updated

## v1.0.12:
- The backend was updated to .NET 8
- The frontend was updated to Angular v17
- The frontend can now display the actual consumption values for the total consumed energy

## v1.0.11:
- Fixed a bug where reducing entries for stats somtimes failed to include the latest entries

## v1.0.10:
- When getting power stats, outliers are now removed. Sometimes, the Modbus protocol gave errorenous readings of hundreds of thousands of watts of thermal power draw

## v1.0.9:
- Fixed getting stats for main advance temperature

## v1.0.8:
- When getting temperature stats, outliers are now removed. Sometimes, the Modbus protocol gave errorenous readings of multiple hundred negative degrees for e.g. the outside temperature

## v1.0.7:
- Updated the internal mapping of the heating circuits
- Added visualization for advance temperature of the heating circuits
- Added data logging for advance temperature of heat exchange station and heating circuits

## v1.0.6:
- The stats endpoint now returns a max of 1000 entries, which will be averaged if more are returned. This is to ensure smooth UI operations

## v1.0.5:
- Fixed a bug where the frontend would not translate times in stats to the correct local time

## v1.0.4:
- User time zone offset provided by the frontend to correctly show user-local data

## v1.0.3:
- Added status for buffer and boiler pumps
- Stats now use local time when displaying them or exporting them to Excel and CSV

## v1.0.2:
- Added a timeout when reading data from the controller unit

## v1.0.1
- Switch to `FluentModus` instead of `NModbus`

## v1.0.0:
- Initial release
