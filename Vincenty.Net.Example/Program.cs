﻿using Vincenty.Net;

internal static class Example
{
    private static void Main()
    {
        TwoDimensionalDirectCalculation();

        Console.WriteLine();

        TwoDimensionalInverseCalculation();

        Console.WriteLine();

        ThreeDimensionalInverseCalculation();

        Console.ReadLine();
    }

    /// <summary>
    /// Calculate the destination if we start at:
    ///    Lincoln Memorial in Washington, D.C --> 38.8892N, 77.04978W
    ///         and travel at
    ///    51.7679 degrees for 6179.016136 kilometers
    /// 
    ///    WGS84 reference ellipsoid
    /// </summary>
    private static void TwoDimensionalDirectCalculation()
    {
        // instantiate the calculator
        GeodeticCalculator geoCalc = new GeodeticCalculator();

        // select a reference elllipsoid
        Ellipsoid reference = Ellipsoid.WGS84;

        // set Lincoln Memorial coordinates
        GlobalCoordinates lincolnMemorial = new GlobalCoordinates(Angle.FromDegrees(38.88922), Angle.FromDegrees(-77.04978));

        // set the direction and distance
        Angle startBearing = Angle.FromDegrees(51.7679);
        double distance = 6179016.13586;

        // find the destination
        Angle endBearing;
        GlobalCoordinates dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, startBearing, distance, out endBearing);

        Console.WriteLine("Travel from Lincoln Memorial at 51.767921 deg for 6179.016 km");
        Console.Write("   Destination: {0:0.0000}{1}", dest.Latitude.Degrees, (dest.Latitude > Angle.Zero) ? "N" : "S");
        Console.WriteLine(", {0:0.0000}{1}", dest.Longitude.Degrees, (dest.Longitude > Angle.Zero) ? "E" : "W");
        Console.WriteLine("   End Bearing: {0:0.00} degrees", endBearing.Degrees);
    }

    /// <summary>
    /// Calculate the two-dimensional path from
    ///    Lincoln Memorial in Washington, D.C --> 38.8892N, 77.04978W
    ///         to
    ///    Eiffel Tower in Paris --> 48.85889N, 2.29583E
    ///         using
    ///    WGS84 reference ellipsoid
    /// </summary>
    private static void TwoDimensionalInverseCalculation()
    {
        // instantiate the calculator
        GeodeticCalculator geoCalc = new GeodeticCalculator();

        // select a reference elllipsoid
        Ellipsoid reference = Ellipsoid.WGS84;

        // set Lincoln Memorial coordinates
        GlobalCoordinates lincolnMemorial = new GlobalCoordinates(Angle.FromDegrees(38.88922), Angle.FromDegrees(-77.04978));

        // set Eiffel Tower coordinates
        GlobalCoordinates eiffelTower = new GlobalCoordinates(Angle.FromDegrees(48.85889), Angle.FromDegrees(2.29583));

        // calculate the geodetic curve
        GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);
        double ellipseKilometers = geoCurve.EllipsoidalDistanceMeters / 1000;
        double ellipseMiles = ellipseKilometers * 0.621371192;

        Console.WriteLine("2-D path from Lincoln Memorial to Eiffel Tower using WGS84");
        Console.WriteLine("   Ellipsoidal Distance: {0:0.00} kilometers ({1:0.00} miles)", ellipseKilometers, ellipseMiles);
        Console.WriteLine("   Azimuth:              {0:0.00} degrees", geoCurve.Azimuth.Degrees);
        Console.WriteLine("   Reverse Azimuth:      {0:0.00} degrees", geoCurve.ReverseAzimuth.Degrees);
    }

    /// <summary>
    /// Calculate the three-dimensional path from
    ///    Pike's Peak in Colorado --> 38.840511N, 105.0445896W, 4301 meters
    ///        to
    ///    Alcatraz Island --> 37.826389N, 122.4225W, sea level
    ///        using
    ///    WGS84 reference ellipsoid
    /// </summary>
    private static void ThreeDimensionalInverseCalculation()
    {
        // instantiate the calculator
        GeodeticCalculator geoCalc = new GeodeticCalculator();

        // select a reference elllipsoid
        Ellipsoid reference = Ellipsoid.WGS84;

        // set Pike's Peak position
        GlobalPosition pikesPeak = new GlobalPosition(new GlobalCoordinates(Angle.FromDegrees(38.840511), Angle.FromDegrees(-105.0445896)), 4301);

        // set Alcatraz Island coordinates
        GlobalPosition alcatrazIsland = new GlobalPosition(new GlobalCoordinates(Angle.FromDegrees(37.826389), Angle.FromDegrees(-122.4225)), 0);

        // calculate the geodetic measurement
        GeodeticMeasurement geoMeasurement;
        double p2pKilometers;
        double p2pMiles;
        double elevChangeMeters;
        double elevChangeFeet;

        geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pikesPeak, alcatrazIsland);
        p2pKilometers = geoMeasurement.PointToPointDistanceMeters / 1000;
        p2pMiles = p2pKilometers * 0.621371192;
        elevChangeMeters = geoMeasurement.ElevationChangeMeters;
        elevChangeFeet = elevChangeMeters * 3.2808399;

        Console.WriteLine("3-D path from Pike's Peak to Alcatraz Island using WGS84");
        Console.WriteLine("   Point-to-Point Distance: {0:0.00} kilometers ({1:0.00} miles)", p2pKilometers, p2pMiles);
        Console.WriteLine("   Elevation change:        {0:0.0} meters ({1:0.0} feet)", elevChangeMeters, elevChangeFeet);
        Console.WriteLine("   Azimuth:                 {0:0.00} degrees", geoMeasurement.Azimuth.Degrees);
        Console.WriteLine("   Reverse Azimuth:         {0:0.00} degrees", geoMeasurement.ReverseAzimuth.Degrees);
    }
}