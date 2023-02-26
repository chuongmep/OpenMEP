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

## GetConnectorSet

```xml
/// <summary>
/// return connector set of element
/// </summary>
/// <param name="element"></param>
/// <returns></returns>
```
![](dyn/pic/Connector.GetConnectorSet.jpg)
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

## GetConnectors

```xml
/// <summary>
/// return list connector from connector manager
/// </summary>
/// <param name="connectorManager">connector manager</param>
/// <returns name="connectors">connectors</returns>
```

![](dyn/pic/Connector.GetConnectors.jpg)
## GetUnusedConnectors

```xml
/// <summary>
/// return list connector from element
/// </summary>
/// <param name="element">element</param>
/// <returns name="connectors">connectors</returns>
```

![](dyn/pic/Connector.GetUnusedConnectors.jpg)
## SystemType

```xml
/// <summary>
/// return system type of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="domain">domain</returns>
```

![](dyn/pic/Connector.SystemType.jpg)

## Direction

```xml
/// <summary>
    /// Get direction of connector
    /// </summary>
    /// <param name="connector"></param>
    /// <returns></returns>
```

![](dyn/pic/Connector.Direction.jpg)

## Origin

```xml
/// <summary>
/// Get origin of connector
/// </summary>
/// <param name="connector">the connector</param>
/// <returns name="point">location origin of connector</returns>
```

![](dyn/pic/Connector.Origin.jpg)
## IsConnected

```xml
/// <summary>
/// check connector is connected or not
/// </summary>
/// <param name="connector">the connector</param>
/// <returns name="bool">true if connector is connected</returns>
```

![](dyn/pic/Connector.IsConnected.jpg)

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

## Id

```xml
/// <summary>
/// return id of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">Id of connector</returns>
```

![](dyn/pic/Connector.Id.jpg)

## Angle

```xml
/// <summary>
/// return angle of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">angle</returns>
```

![](dyn/pic/Connector.Angle.jpg)

## Coefficient

```xml
/// <summary>
/// The coefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Coefficient</returns>
```

![](dyn/pic/Connector.Coefficient.jpg)

## Demand

```xml
/// <summary>
/// The demand of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Demand</returns>
```

![](dyn/pic/Connector.Demand.jpg)

## Flow

```xml
/// <summary>
/// The Flow of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Flow of connector</returns>
```

![](dyn/pic/Connector.Flow.jpg)

## Height

```xml
/// <summary>
/// The height of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Height</returns>
```

![](dyn/pic/Connector.Height.jpg)

## Width

```xml
/// <summary>
/// The Width of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">the width of connector</returns>
```
![](dyn/pic/Connector.Width.jpg)

## AssignedFlow

```xml
/// <summary>
/// The assigned flow of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFlow</returns>
```

![](dyn/pic/Connector.AssignedFlow.jpg)

## EngagementLength

```xml
/// <summary>
/// Connector engagement length
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">Connector engagement length</returns>
```

![](dyn/pic/Connector.EngagementLength.jpg)
## PressureDrop

```xml
/// <summary>
/// The pressure drop of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">PressureDrop</returns>
```


## AllRefs

```xml
/// <summary>
/// All references of the connector.
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="connectors">A set of connectors that the connectors is connected to, including both physical connection and logical connection.</returns>
```

![](dyn/pic/Connector.AllRefs.jpg)

## Domain

```xml
/// <summary>
/// The domain of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="Autodesk.Revit.DB.Domain">Domain</returns>
```

![](dyn/pic/Connector.Domain.jpg)

## VelocityPressure

```xml
/// <summary>
/// The velocity pressure of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">VelocityPressure</returns>
```

![](dyn/pic/Connector.VelocityPressure.jpg)
## AssignedFixtureUnits

```xml
/// <summary>
/// The assigned fixture units of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFixtureUnits</returns>
```

![](dyn/pic/Connector.AssignedFixtureUnits.jpg)

## AssignedFlowFactor

```xml
/// <summary>
/// The assigned flow factor of  connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedFlowFactor</returns>
```

![](dyn/pic/Connector.AssignedFlowFactor.jpg)
## AssignedKCoefficient

```xml
/// <summary>
/// The assigned kCoefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedKCoefficient</returns>
```

![](dyn/pic/Connector.AssignedKCoefficient.jpg)

## GetElementConnectedWith

```xml
/// <summary>
/// return element connected with  connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="element">element has connected with connector</returns>
```

![](dyn/pic/Connector.GetElementConnectedWith.jpg)
## AssignedLossCoefficient

```xml
/// <summary>
/// The assigned loss coefficient of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedLossCoefficient</returns>
```

![](dyn/pic/Connector.AssignedKCoefficient.jpg)

## AssignedPressureDrop

```xml
/// <summary>
/// The assigned pressure drop of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="double">AssignedPressureDrop</returns>
```

![](dyn/pic/Connector.AssignedPressureDrop.jpg)

## Description

```xml
/// <summary>
/// The description of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="string">Description</returns>
```
![](dyn/pic/Connector.Description.jpg)
## Shape

```xml
/// <summary>
/// The shape of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="ConnectorProfileType">ConnectorProfileType</returns>
```

![](dyn/pic/Connector.Shape.jpg)

## CoordinateSystem

```xml
/// <summary>
/// Return CoordinateSystem of the connector.
/// </summary>
/// <param name="connector"></param>
/// <returns></returns>
```

![](dyn/pic/Connector.CoordinateSystem.jpg)
## ConnectorManager

```xml
/// <summary>
/// The connector manager of the connector.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="ConnectorManager">ConnectorManager</returns>

```
![](dyn/pic/Connector.ConnectorManager.jpg)

## GetArea

```xml
/// <summary>
/// Get area of connector
/// </summary>
/// <param name="connector">connector</param>
/// <returns name="double">area of connector</returns>
```
![](dyn/pic/Connector.GetArea.jpg)
## GetMEPConnectorInfo

```xml
/// <summary>
/// Gets MEP connector information.
/// </summary>
/// <param name="connector">Connector</param>
/// <returns name="MEPConnectorInfo">Returns null if there is no MEP connector information associated</returns>
```

![](dyn/pic/Connector.GetMEPConnectorInfo.jpg)

## SetAngle

```xml
/// <summary>
/// set new angle for connector
/// </summary>
/// <param name="connector">connector</param>
/// <param name="angle">angle</param>
```

![](dyn/pic/Connector.SetAngle.jpg)
## SetOrigin

```xml
/// <summary>
/// set new origin for connector
/// </summary>
/// <param name="connector">connector</param>
/// <param name="origin">new point</param>
```

![](dyn/pic/Connector.SetOrigin.jpg)
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