# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.1] - 2019-03-11
### Changed
- Change license from apache-2.0 to mpl-2.0

### Fixed
- You don't lose food when you go into outer walls anymore
- Android is now in enforced portrait mode (working on making auto rotation possible)
- Enemies can't kill you when on exit sign thanks to immortal on player.
- When restarting, level and food will be the same as when the game was first loaded

## [1.1.0] - 2019-03-09
### Added
- Restart option at game over
- User report system
- This CHANGELOG

### Fixed
- Support for Standalone/WebGL with touchscreen

### Security
- Replaced deprecated OnLevelWasLoaded with sceneLoaded event

## 1.0.0 - 2019-03-08
### Added
- Full working 2D Roguelike Unity tutorial

[Unreleased]: https://github.com/RomainL972/Scavengers/compare/v1.1.0...HEAD
[1.1.0]: https://github.com/RomainL972/Scavengers/compare/v1.0.0...v1.1.0