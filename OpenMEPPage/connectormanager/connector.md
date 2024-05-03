# Connector

![](dyn/pic/Connector.png)

```{contents}
```

## Radius

```xml
/// <summary>
/// return the radius of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="radius">radius</returns>
```

![](dyn/pic/Connector.Radius.jpg)

[Connector.Radius.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Radius.dyn)

## GetConnectorClosest

```xml
/// <summary>
///  Return Connector Closet With Connector Current
/// </summary>
/// <param name="point">origin</param>
/// <param name="connectors">list connector to check</param>
/// <returns name="connector">connector</returns>
```

![](dyn/pic/Connector.GetConnectorClosest.jpg)

[Connector.GetConnectorClosest.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetConnectorClosest.dyn)
## GetConnectorSet

```xml
/// <summary>
/// return connector set of element
/// </summary>
/// <param name="element"></param>
/// <returns></returns>
```
![](dyn/pic/Connector.GetConnectorSet.jpg)

[Connector.GetConnectorSet.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetConnectorSet.dyn)
## GetConnectorFarthest

```xml
/// <summary>
/// return connector farthest with point current
/// </summary>
/// <param name="point"></param>
/// <param name="connectors"></param>
/// <returns></returns>
```

![](dyn/pic/Connector.GetConnectorFarthest.jpg)

[Connector.GetConnectorFarthest.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetConnectorFarthest.dyn)

## GetConnectors

```xml
/// <summary>
/// return list connector from connector manager
/// </summary>
/// <param name="connectorManager">connector manager</param>
/// <returns name="connectors">connectors</returns>
```

![](dyn/pic/Connector.GetConnectors.jpg)

[Connector.GetConnectors.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetConnectors.dyn)

## GetUnusedConnectors

```xml
/// <summary>
/// return list connector from element
/// </summary>
/// <param name="element">element</param>
/// <returns name="connectors">connectors</returns>
```

![](dyn/pic/Connector.GetUnusedConnectors.jpg)

[Connector.GetUnusedConnectors.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetUnusedConnectors.dyn)
## SystemType

```xml
/// <summary>
/// return system type of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="domain">domain</returns>
```

![](dyn/pic/Connector.SystemType.jpg)

[Connector.SystemType.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.SystemType.dyn)

## Direction

```xml
/// <summary>
    /// Get direction of connector
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
```

![](dyn/pic/Connector.Direction.jpg)

[Connector.Direction.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Direction.dyn)

## Origin

```xml
/// <summary>
/// Get origin of connector
/// </summary>
/// <param name="connector">the connector</param>
/// <returns name="point">location origin of connector</returns>
```

![](dyn/pic/Connector.Origin.jpg)

[Connector.Origin.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Origin.dyn)
## IsConnected

```xml
/// <summary>
/// check connector is connected or not
/// </summary>
/// <param name="connector">the connector</param>
/// <returns name="bool">true if connector is connected</returns>
```

![](dyn/pic/Connector.IsConnected.jpg)

[Connector.IsConnected.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.IsConnected.dyn)

## DistanceTo

```xml
/// <summary>
/// return distance between one connector with another point
/// </summary>
/// <param name="connector">the connector</param>
/// <param name="point">point to get distance from this to the connector</param>
/// <returns name="double">distance from connector to point</returns>
```

![](dyn/pic/Connector.DistanceTo.jpg)

[Connector.DistanceTo.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.DistanceTo.dyn)

## Owner

```xml
/// <summary>
/// The host of the connector.
/// The element that contains  connector. It may also contain other connectors.
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="element">element</returns>
```

![](dyn/pic/Connector.Owner.jpg)

[Connector.Owner.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Owner.dyn)

## Id

```xml
/// <summary>
/// return id of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">Id of connector</returns>
```

![](dyn/pic/Connector.Id.jpg)

[Connector.Id.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Id.dyn)

## Angle

```xml
/// <summary>
/// return angle of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">angle</returns>
```

![](dyn/pic/Connector.Angle.jpg)

[Connector.Angle.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Angle.dyn)

## Coefficient

```xml
/// <summary>
/// The coefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Coefficient</returns>
```

![](dyn/pic/Connector.Coefficient.jpg)

[Connector.Coefficient.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Coefficient.dyn)

## Demand

```xml
/// <summary>
/// The demand of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Demand</returns>
```

![](dyn/pic/Connector.Demand.jpg)

[Connector.Demand.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Demand.dyn)
## Flow

```xml
/// <summary>
/// The Flow of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Flow of connector</returns>
```

![](dyn/pic/Connector.Flow.jpg)

[Connector.Flow.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Flow.dyn)

## Height

```xml
/// <summary>
/// The height of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Height</returns>
```

![](dyn/pic/Connector.Height.jpg)

[Connector.Height.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Height.dyn)

## Width

```xml
/// <summary>
/// The Width of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">the width of connector</returns>
```
![](dyn/pic/Connector.Width.jpg)

[Connector.Width.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Width.dyn)
## AssignedFlow

```xml
/// <summary>
/// The assigned flow of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFlow</returns>
```

![](dyn/pic/Connector.AssignedFlow.jpg)

[Connector.AssignedFlow.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedFlow.dyn)
## EngagementLength

```xml
/// <summary>
/// Connector engagement length
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Connector engagement length</returns>
```

![](dyn/pic/Connector.EngagementLength.jpg)

[Connector.EngagementLength.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.EngagementLength.dyn)
## PressureDrop

```xml
/// <summary>
/// The pressure drop of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">PressureDrop</returns>
```
![](dyn/pic/Connector.PressureDrop.png)

[Connector.PressureDrop.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.PressureDrop.dyn)

## AllRefs

```xml
/// <summary>
/// All references of the connector.
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="connectors">A set of connectors that the connectors is connected to, including both physical connection and logical connection.</returns>
```

![](dyn/pic/Connector.AllRefs.jpg)

[Connector.AllRefs.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AllRefs.dyn)

## Domain

```xml
/// <summary>
/// The domain of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="Autodesk.Revit.DB.Domain">Domain</returns>
```

![](dyn/pic/Connector.Domain.jpg)

[Connector.Domain.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Domain.dyn)

## VelocityPressure

```xml
/// <summary>
/// The velocity pressure of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">VelocityPressure</returns>
```

![](dyn/pic/Connector.VelocityPressure.jpg)

[Connector.VelocityPressure.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.VelocityPressure.dyn)
## AssignedFixtureUnits

```xml
/// <summary>
/// The assigned fixture units of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFixtureUnits</returns>
```

![](dyn/pic/Connector.AssignedFixtureUnits.jpg)

[Connector.AssignedFixtureUnits.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedFixtureUnits.dyn)

## AssignedFlowFactor

```xml
/// <summary>
/// The assigned flow factor of  connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFlowFactor</returns>
```

![](dyn/pic/Connector.AssignedFlowFactor.jpg)

[Connector.AssignedFlowFactor.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedFlowFactor.dyn)
## AssignedKCoefficient

```xml
/// <summary>
/// The assigned kCoefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedKCoefficient</returns>
```

![](dyn/pic/Connector.AssignedKCoefficient.jpg)

[Connector.AssignedKCoefficient.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedKCoefficient.dyn)

## GetElementConnectedWith

```xml
/// <summary>
/// return element connected with  connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="element">element has connected with connector</returns>
```

![](dyn/pic/Connector.GetElementConnectedWith.jpg)

[Connector.GetElementConnectedWith.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetElementConnectedWith.dyn)

## AssignedLossCoefficient

```xml
/// <summary>
/// The assigned loss coefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedLossCoefficient</returns>
```

![](dyn/pic/Connector.AssignedKCoefficient.jpg)

[Connector.AssignedKCoefficient.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedKCoefficient.dyn)

## AssignedPressureDrop

```xml
/// <summary>
/// The assigned pressure drop of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedPressureDrop</returns>
```

![](dyn/pic/Connector.AssignedPressureDrop.jpg)

[Connector.AssignedPressureDrop.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.AssignedPressureDrop.dyn)

## Description

```xml
/// <summary>
/// The description of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="string">Description</returns>
```
![](dyn/pic/Connector.Description.jpg)

[Connector.Description.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Description.dyn)

## Shape

```xml
/// <summary>
/// The shape of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="ConnectorProfileType">ConnectorProfileType</returns>
```

![](dyn/pic/Connector.Shape.jpg)

[Connector.Shape.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.Shape.dyn)

## CoordinateSystem

```xml
/// <summary>
/// Return CoordinateSystem of the connector.
/// </summary>
/// <param name="connector"></param>
/// <returns></returns>
```

![](dyn/pic/Connector.CoordinateSystem.jpg)

[Connector.CoordinateSystem.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.CoordinateSystem.dyn)
## ConnectorManager

```xml
/// <summary>
/// The connector manager of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="ConnectorManager">ConnectorManager</returns>

```
![](dyn/pic/Connector.ConnectorManager.jpg)

[Connector.ConnectorManager.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.ConnectorManager.dyn)

## GetArea

```xml
/// <summary>
/// Get area of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">area of connector</returns>
```
![](dyn/pic/Connector.GetArea.jpg)

[Connector.GetArea.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetArea.dyn)
## GetMEPConnectorInfo

```xml
/// <summary>
/// Gets MEP connector information.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="MEPConnectorInfo">Returns null if there is no MEP connector information associated</returns>
```

![](dyn/pic/Connector.GetMEPConnectorInfo.jpg)

[Connector.GetMEPConnectorInfo.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.GetMEPConnectorInfo.dyn)

## SetAngle

```xml
/// <summary>
/// set new angle for connector
/// </summary>
/// <param name="connector">connector</param>
/// <param name="angle">angle</param>
```

![](dyn/pic/Connector.SetAngle.jpg)

[Connector.SetAngle.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.SetAngle.dyn)

## SetOrigin

```xml
/// <summary>
/// set new origin for connector
/// </summary>
/// <param name="connector">connector</param>
/// <param name="origin">new point</param>
```

![](dyn/pic/Connector.SetOrigin.jpg)

[Connector.SetOrigin.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.SetOrigin.dyn)

## DisConnectFrom

```xml
/// <summary>
/// Remove connection between two connectors.
/// </summary>
/// <param name="connector">connect need disconnect</param>
/// <param name="connectorFrom">Indicate the connector, connection will be removed from.</param>
/// <returns name="connector">connector need disconnect</returns>
```

![](dyn/pic/Connector.DisConnectFrom.jpg)

[Connector.DisConnectFrom.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.DisConnectFrom.dyn)

## ConnectTo

```xml
/// <summary>
/// Remove connection between two connectors.
/// </summary>
/// <param name="connector">connect need connect</param>
/// <param name="connectorFrom">Indicate the connector, connection will be removed from.</param>
/// <returns name="connector">connect need connect</returns>
```

![](dyn/pic/Connector.ConnectTo.jpg)

[Connector.ConnectTo.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/connectormanager/dyn/Connector.ConnectTo.dyn)


## Display

```xml
/// <summary>
/// Shows scalable lines representing the CoordinateSystem of a Connector.
/// </summary>
/// <param name="connector">Autodesk.Revit.DB.Connector</param>
/// <param name="length">double</param>
/// <returns name="Display">GeometryColor order by x,y,z</returns>
/// <returns name="Origin">Point</returns>
/// <returns name="XAxis">Vector(Red color)</returns>
/// <returns name="YAxis">Vector(Green color)</returns>
/// <returns name="ZAxis">Vector(Blue color)</returns>
/// <returns name="XYPlane">Plane(Red-Green color)</returns>
/// <returns name="YZPlane">Plane(Green-Blue color)</returns>
/// <returns name="ZXPlane">Plane(Blue-Red color)</returns>
```

![](dyn/pic/Connector.Display.gif)
