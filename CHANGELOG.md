# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.50] - 2021-09-02
### Changed
- NetUtils namespace
### Added
- Net50 support
- Net60 support

## [0.1.44] - 2020-12-20
### Changed
- NetUtils.HttpClientEntity: changed Open to OpenAsync method
- NetUtils.PingEntity: added BufferSize, Ttl, DontFragment
- NetUtils.PingEntity: fixed errors
- NetUtilsTests.HttpClientEntityTests

## [0.1.40] - 2020-12-10
### Changed
- Changed target platform of projects
- NetUtils.PingEntity: changed Status property to Log and Settings
### Added
- Multi-targeted platforms
  - netstandard2.0
  - netstandard2.1
  - net45
  - net46
  - net461
  - net472
  - net48

## [0.1.34] - 2020-11-23
### Changed
- NetUtils.PingEntity: fixed catch exceptions and async method, added info
- NetUtils.Tests.PingEntityTests: fixed catch exceptions and async method

## [0.1.31] - 2020-11-22
### Changed
- NetUtils.HttpClientEntity: fixed INotifyPropertyChanged
- NetUtils.PingEntity: fixed INotifyPropertyChanged

## [0.1.30] - 2020-11-18
### Changed
- NetUtils.HttpClientEntity: fixed bugs
- NetUtils.PingEntity: fixed bugs
- NetUtils.Tests.HttpClientEntityTests
- NetUtils.Tests.PingEntityTests

## [0.1.21] - 2020-11-08
### Added
- NetUtils.PingEntity
- NetUtils.Tests.PingEntityTests
### Changed
- NetUtils.HttpClientEntity
- NetUtils.Tests.HttpClientEntityTests

## [0.1.13] - 2020-11-07
### Added
- NetUtils project
- NetUtils.HttpClientEntity
- NetUtils.ProxyEntity
- NetUtils.Tests project
- NetUtils.Tests.HttpClientEntityTests
- NetUtils.Tests.EnumValues
- NetUtils.Tests.ProxyEntityTests
- NetUtils.Tests.Utils
