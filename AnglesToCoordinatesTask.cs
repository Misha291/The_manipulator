using System;
using Avalonia;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
	/// <summary>
	/// По значению углов суставов возвращает массив координат суставов
	/// в порядке new []{elbow, wrist, palmEnd}
	/// </summary>
	public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
	{
		var upperArmX = UpperArm*Math.Cos(shoulder);
		var upperArmY = UpperArm*Math.Sin(shoulder);
		var elbowPos = new Point(upperArmX, upperArmY);

		var currentElbow = shoulder + elbow - Math.PI; 
        var forearmX = Forearm * Math.Cos(currentElbow) + upperArmX;
        var forearmY = Forearm * Math.Sin(currentElbow) + upperArmY;
        var wristPos = new Point(forearmX, forearmY);

		var currentWrist = wrist + currentElbow - Math.PI;
		var palmX = Palm * Math.Cos(currentWrist) + forearmX;
		var palmY = Palm * Math.Sin(currentWrist) + forearmY;
		var palmEndPos = new Point(palmX, palmY);
		return new[]
		{
			elbowPos,
			wristPos,
			palmEndPos
		};
	}
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
    [TestCase(0, Math.PI, Math.PI, UpperArm + Forearm + Palm, 0)] 
    [TestCase(Math.PI, Math.PI, Math.PI, -UpperArm - Forearm - Palm, 0)]
    [TestCase(Math.PI / 2, Math.PI, 0, 0, UpperArm + Forearm - Palm)]
    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
        Assert.That(joints[2].X, Is.EqualTo(palmEndX).Within(1e-5), "palm endX");
        Assert.That(joints[2].Y, Is.EqualTo(palmEndY).Within(1e-5), "palm endY");
	}
}