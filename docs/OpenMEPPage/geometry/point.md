# Point

```{contents}

```

## ProjectOntoPlane

```xml
/// <summary>
/// Project a point onto a plane
/// </summary>
/// <param name="point">point need to project</param>
/// <param name="planeNormal">vector normal of plane</param>
/// <returns name="point">new point projected on plane</returns>
```

`Example` :

![ProjectOntoPlane](dyn/pic/Point.ProjectOntoPlane.gif)

[ProjectOntoPlane.dyn](https://github.com/chuongmep/OpenMEP/blob/dev/docs/OpenMEPPage/geometry/dyn/Point.ProjectOntoPlane.dyn)

## Centroid

```xml
/// <summary>
/// Get the centroid of a list of points
/// </summary>
/// <param name="points">list of points</param>
/// <returns name="point">centroid</returns>
```

![Centroid](dyn/pic/Point.Centroid.gif)

## Deconstruct

```xml
/// <summary>
///  Deconstruct a point into its components
/// </summary>
/// <param name="point">the point</param>
/// <returns name="X">X point</returns>
/// <returns name="Y">Y point</returns>
/// <returns name="Z">Z point</returns>
```

![Deconstruct](dyn/pic/Point.Deconstruct.png)

## IsOnPlane

```xml
/// <summary>Test whether a point lies on a plane.</summary>
/// <param name="point">point to check</param>
/// <param name="plane">The plane to test against.</param>
/// <param name="tolerance">Default is use 1e-6</param>
/// <returns>Returns true if point is on plane.</returns>
```

![IsOnPlane](dyn/pic/Point.IsOnPlane.gif)

## IsOnLine

```xml
/// <summary>Test whether a point lies on a line.</summary>
/// <param name="point">a point to check</param>
/// <param name="line">The line to test against.</param>
/// <param name="tolerance">Default is use 1e-6</param>
/// <returns name="bool">Returns true if point is on line.</returns>
```

![IsOnLine](dyn/pic/Point.IsOnLine.gif)

## IsInPolygon

```xml
/// <summary>
/// Returns whether an input point is contained within the polygon. If the polygon is not planar then the point
/// will be projected onto the best-fit plane and the containment will be computed using the projection of the polygon
/// onto the best-fit plane. This will return a failed status if the polygon self-intersects.
/// </summary>
/// <param name="point">the point</param>
/// <param name="polygon">the polygon</param>
/// <returns name="bool">true if point is in polygon</returns>
/// <exception cref="ArgumentNullException"></exception>
```

![IsInPolygon](dyn/pic/Point.IsInPolygons.gif)
