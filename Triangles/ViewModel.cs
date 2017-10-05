/*=========================================================================================
                          Your company name here
                    Copyright (c) 2017. All rights reserved.
         This document contains confidential/proprietary information...
                    ...and other corporate/developmental stuff
===========================================================================================
  Workfile: ViewModel.cs
  10/05/2017 by dek

  File Description:  Sample triangle program as part of employemnt application to Cherwell.
  NOTES: GetTriangleFromPoints allows for duplicate point entries when searching for a
         triangle in the map.
=========================================================================================*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace Triangles
{
  public class ViewModel : INotifyPropertyChanged
  {
    #region Members

    /// <summary>
    /// Triangle class that represents each triangle in the map
    /// </summary>
    private class Triangle
    {
      public string Name;
      public char Row;
      public int Column;
      public Point Point1;
      public Point Point2;
      public Point Point3;
    }

    // triangle leg lengths
    private const int _verticalLegLength = 10;
    private const int _horizontalLegLength = 10;

    #endregion

    #region Properties

    /// <summary>
    /// Row count (A-Z)  default is 'F'
    /// </summary>
    private string _rowCount = "F";
    public string RowCount
    {
      get { return _rowCount; }
      set
      {
        _rowCount = value;
        ComputeCommand.InvalidateCanExecute();
        RaisePropertyChanged("RowCount");
      }
    }

    /// <summary>
    /// Column count (1-99) default is 12
    /// </summary>
    private int? _columnCount = 12;
    public int? ColumnCount
    {
      get { return _columnCount; }
      set
      {
        _columnCount = value;
        ComputeCommand.InvalidateCanExecute();
        RaisePropertyChanged("ColumnCount");
      }
    }

    /// <summary>
    /// Results string rendered in form
    /// </summary>
    private string _results;
    public string Results
    {
      get { return _results; }
      set
      {
        _results = value;
        RaisePropertyChanged("Results");
      }
    }

    /// <summary>
    /// vertex properties for search triangle
    /// </summary>
    private int? _vertex1X;
    public int? Vertex1X
    {
      get { return _vertex1X; }
      set
      {
        _vertex1X = value;
        RaisePropertyChanged("Vertex1X");
      }
    }

    private int? _vertex1Y;
    public int? Vertex1Y
    {
      get { return _vertex1Y; }
      set
      {
        _vertex1Y = value;
        RaisePropertyChanged("Vertex1Y");
      }
    }

    private int? _vertex2X;
    public int? Vertex2X
    {
      get { return _vertex2X; }
      set
      {
        _vertex2X = value;
        RaisePropertyChanged("Vertex2X");
      }
    }

    private int? _vertex2Y;
    public int? Vertex2Y
    {
      get { return _vertex2Y; }
      set
      {
        _vertex2Y = value;
        RaisePropertyChanged("Vertex2Y");
      }
    }

    private int? _vertex3X;
    public int? Vertex3X
    {
      get { return _vertex3X; }
      set
      {
        _vertex3X = value;
        RaisePropertyChanged("Vertex3X");
      }
    }

    private int? _vertex3Y;
    public int? Vertex3Y
    {
      get { return _vertex3Y; }
      set
      {
        _vertex3Y = value;
        RaisePropertyChanged("Vertex3Y");
      }
    }

    /// <summary>
    /// command handler for "Compute" button
    /// </summary>
    public DelegateCommand ComputeCommand { get; set; }

    #endregion

    #region Constructor(s)

    public ViewModel()
    {
      ConstructCommands();
    }

    #endregion

    #region Command Handlers

    /// <summary>
    /// Constructs command handler for "Compute" button
    /// </summary>
    private void ConstructCommands()
    {
      ComputeCommand = new DelegateCommand(ComputeCommandAction, CanCompute);
    }

    /// <summary>
    /// Enables the "Compute button if RowCont and ColumnCount are valid values
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    private bool CanCompute(object arg)
    {
      return ! string.IsNullOrEmpty(RowCount) && ColumnCount != null;
    }

    /// <summary>
    /// Command handler for "Compute" button
    /// </summary>
    private void ComputeCommandAction(object arg)
    {
      string results = string.Empty;

      try
      {
        // build the map
        var triangleList = BuildTriangleMap();

        // loop through list of triangles, build the results string
        foreach (var triangle in triangleList)
        {
          results += "Triangle Name: " + triangle.Name +
                     "  Row: " + triangle.Row +
                     "  Column: " + triangle.Column +
                     "  Pt1: " + triangle.Point1.X + "," + triangle.Point1.Y +
                     "  Pt2: " + triangle.Point2.X + "," + triangle.Point2.Y +
                     "  Pt3: " + triangle.Point3.X + "," + triangle.Point3.Y +
                     Environment.NewLine;
        }

        if (IsSearchingTriangle())
        {
          // create search points from vertex properties
          var pt1 = new Point((int)Vertex1X, (int)Vertex1Y);
          var pt2 = new Point((int)Vertex2X, (int)Vertex2Y);
          var pt3 = new Point((int)Vertex3X, (int)Vertex3Y);

          // get it!
          var searchTriangle = GetTriangleFromPoints(triangleList, pt1, pt2, pt3);

          results += searchTriangle != null ? "FOUND TRIANGLE NAME: " + searchTriangle.Name : "TRIANGLE NOT FOUND";
        }
        else
        {
          results += "TRIANGLE SEARCH NOT PERFORMED (missing values)";
        }
      }
      catch (Exception ex)
      {
        // any error is rendered in results box
        results = "ERROR: " + ex.Message;
      }

      Results = results;
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Builds the map of triangles based on given total
    /// row and column settings provided by the user.  
    /// Point location is determined counter-clockwise.
    /// </summary>
    /// <returns>the list of triangles that compose the map</returns>
    private List<Triangle> BuildTriangleMap()
    {
      var triangleList = new List<Triangle>();

      int currentX = 0;
      int currentY = 0;

      // covert upper/lower case row letter to equivalent number
      int rowCount = RowLetterToRowNumber(RowCount[0]);

      // loop through all rows and columns, generating triangles
      for (int row = 1; row <= rowCount; row++)
      {
        for (int column = 1; column <= ColumnCount; column++)
        {
          bool isColEven = column % 2 == 0;

          // calculate 2nd and 3rd points
          var point2 = new Point(isColEven ? currentX + _horizontalLegLength : currentX, currentY + _verticalLegLength);
          var point3 = new Point(currentX + _horizontalLegLength, isColEven ? currentY : currentY + _verticalLegLength);

          // get row letter equivalent to row number
          char rowChar = RowNumberToRowLetter(row);

          // add new triangle to the list
          triangleList.Add(new Triangle()
          {
            Name = rowChar.ToString() + column,
            Row = RowNumberToRowLetter(row),
            Column = column,
            Point1 = new Point(currentX, currentY),
            Point2 = point2,
            Point3 = point3
          });

          // step x
          if (isColEven)
          {
            currentX += _horizontalLegLength;
          }
        }

        // reset x, step y
        currentX = 0;
        currentY += _verticalLegLength;
      }

      return triangleList;
    }

    /// <summary>
    /// Searches the map for a triangle that matches the given vertices
    /// </summary>
    /// <param name="triangleList">the generated triangle map</param>
    /// <param name="pt1">point 1 for search</param>
    /// <param name="pt2">point 2 for search</param>
    /// <param name="pt3">point 3 for search</param>
    /// <returns>the found triangle, otherwise null</returns>
    private Triangle GetTriangleFromPoints(List<Triangle> triangleList, Point pt1, Point pt2, Point pt3)
    {
      return triangleList.FirstOrDefault(t => (t.Point1 == pt1 || t.Point1 == pt2 || t.Point1 == pt3) &&
                                              (t.Point2 == pt1 || t.Point2 == pt2 || t.Point2 == pt3) &&
                                              (t.Point3 == pt1 || t.Point3 == pt2 || t.Point3 == pt3));
    }

    /// <summary>
    /// Returns whether or not all vertex values for search triangle are valid
    /// </summary>
    /// <returns>True to search for triangle, otherwsie false</returns>
    private bool IsSearchingTriangle()
    {
      return Vertex1X != null && Vertex1Y != null &&
             Vertex2X != null && Vertex2Y != null &&
             Vertex3X != null && Vertex3Y != null;
    }

    /// <summary>
    /// Converts upper/lower case row letter (A-Z, a-z) to a sequential number (1-26)
    /// </summary>
    /// <param name="letter">the char to convert</param>
    /// <returns>a number representing the character position in the alphabet</returns>
    private int RowLetterToRowNumber(char letter)
    {
      return letter > 'Z' ? (int)RowCount[0] - 96 : (int)RowCount[0] - 64;
    }

    /// <summary>
    /// Converts a number to a letter in the alphabet at that position
    /// </summary>
    /// <param name="number">the number to convert</param>
    /// <returns>upper case (A-Z) corresponding to the number postion in the alphabet</returns>
    private char RowNumberToRowLetter(int number)
    {
      return (char)(number + 64);
    }

    #endregion

    #region IINotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged(string propertyName)
    {
      OnPropertyChanged(propertyName);
    }

    protected void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
