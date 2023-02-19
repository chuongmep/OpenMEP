# Open MEP (WIP)

![Revit API](https://img.shields.io/badge/Revit%20API%202023-blue.svg) ![Platform](https://img.shields.io/badge/platform-Windows-lightgray.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![ReSharper](https://img.shields.io/badge/ReSharper-2022-yellow) ![Rider](https://img.shields.io/badge/Rider-2022-yellow) ![Visual Studio 2022](https://img.shields.io/badge/Visual_Studio_2022-yellow) ![.NET Framework](https://img.shields.io/badge/.NET_6.0-yellow)

[![Publish](../../actions/workflows/Workflow.yml/badge.svg)](../../actions)
[![Github All Releases](https://img.shields.io/github/downloads/chuongmep/openmep/total?color=blue&label=Download)]()
[![HitCount](https://hits.dwyl.com/chuongmep/openmep.svg?style=flat-square)](http://hits.dwyl.com/chuongmep/openmep)
<a href="https://twitter.com/intent/follow?screen_name=chuongmep">
<img src="https://img.shields.io/twitter/follow/chuongmep?style=social&logo=twitter"
alt="follow on Twitter"></a>

![](docs/img/openmep.png)

# Description

The MEP Package integrates advanced MEP design and analysis capabilities, allowing construction professionals to quickly
and accurately evaluate MEP systems, from concept to construction. The package includes tools for electrical load
analysis, duct and pipe sizing, pressure drop calculations, and more. This makes it possible for you to quickly identify
potential problems and make informed decisions about the best course of action.

OpenMEP Package also includes a comprehensive library of MEP components, making it easy to select and incorporate the
right components into your design.This library includes a wide range of mechanical, electrical, and plumbing components,
including pipes, fittings, valves, ducts, electrical equipment, and more fully automate your design process in design,
maintenance, calculation and analysis,...

We believe that the MEP Package will be a valuable asset to construction professionals looking to streamline the MEP
design process and ensure that their projects are completed on time and within budget.

# Installation

The package installer is available on the [Open MEP Release](https://github.com/chuongmep/OpenMEP/releases/latest). You
can install it from there.

- OpenMEP now Support Revit Version : 2020, 2021, 2022 , 2023
- OpenMEP now Dynamo Version : 2.6, 2.12, 2.16

![](docs/img/install.png)

# Features

Some features of this package:

- [x] Fast delivery package for MEP Engineer
- [x] Easy to use and interactive with Dynamo Revit
- [x] Easy to collaborate with other engineer
- [x] Easy to report bug
- [x] Easy to maintain, support multiple version of Dynamo Revit
- [x] Easy install, update, uninstall

Function Support (WIP) :

- [x] Document
    - [x] Revit Document
    - [x] Family Document
- [x] ConnectorManager
    - [x] Connector
    - [x] ConnectorSet
    - [x] MEPConnectorInfo
    - [x] MEPModel
- [x] Element
- [x] Duct
- [ ] Pipe
- [x] Cable Tray
- [x] Fitting
- [x] Flex Duct
- [x] Flex Pipe
- [x] Family
    - [x] Family Manager
    - [x] Family Parameter
    - [x] Family Size Manager
    - [x] Family Type
- [x] MEP Curve
- [x] MEP Model
- [x] Utils
- [ ] Wire
- [ ] Geometry
    - [ ] Point
    - [ ] Line
    - [ ] Plane
    - [ ] Curve
    - [ ] Surface
    - [ ] Solid
    - [ ] Mesh
    - [ ] Brep
    - [ ] Transform
    - [ ] Vector
- [ ] Conduit
- [ ] MEP Fabrication
- [ ] ...

# Copyright

This package is licensed under the [MIT License](LICENSE.md).

# Sponsors

- This package is sponsored by [Jetbrains](https://www.jetbrains.com/?from=OpenMEP), the best IDE for C# and Python
  developer.
- Chicken icon made by [icons8.com](https://icons8.com/)

- [Exyte](https://www.exyte.net/en) is a global leader in design, engineering and delivery of facilities for high-tech
  industries.

# Contributing

I have a lot of ideas for this package, but I don't have enough time to implement them. If you want to contribute,
please read guideline [here](CONTRIBUTING.md).

Many Thanks all contributors for this repository. Feel free to contribute!
Please refer to the [CONTRIBUTING](CONTRIBUTING.md) for details.

<a href = "https://github.com/chuongmep/openmep/graphs/contributors">
  <img src = "https://contrib.rocks/image?repo=chuongmep/openmep"/>
</a>

# Issues

Now, I accept all idea and all issue, contribute from all people all the word.
You can make suggestions or track and submit bugs via [OpenMEP](https://github.com/chuongmep/OpenMEP/issues) issues. You can submit your own code to the Add-In Manager
project via a Github pull [request](https://github.com/chuongmep/OpenMEP/pulls).

# FAQ

Why this package not published on Dynamo Package Manager?

- Answer : It not support way I maintain with multiple version and release CI/CD with Dynamo Revit.
