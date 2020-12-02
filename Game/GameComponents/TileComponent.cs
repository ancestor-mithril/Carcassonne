using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameComponents;
using ArrayAccessMethods;


namespace GameComponents {


    public class TileComponent {
    public string Name { get; set; }
    public int[,] Matrix { get; set; }
    public List<ComponentType> Types { get; set; }
    

    public TileComponent(string name, int[,] matrix, List<ComponentType> types) {
      this.Name = name;
      this.Matrix = matrix;
      this.Types = types;
    }


    public TileComponent(TileComponent another) {
      this.Name = another.Name;
      this.Matrix = (int[,]) another.Matrix.Clone();
      this.Types = another.CloneTypes();
    }


    public List<ComponentType> CloneTypes() {
      List<ComponentType> returnList = new List<ComponentType>();
      foreach (var i in this.Types) {
        returnList.Add(i.Clone());
      }
      return returnList;
    }


    public TileComponent Clone() {
      return new TileComponent(this);
    }



    /**
    * returns a 5 x 5 matrix of "road", "field" and/or "city" for neigborhooding purposes
    */
    public string[,] GetCharacteristicMatrix() {
      //! aici ar putea fi fain un overloading, pentru a primi matricea si string si int
      var returnMatrix = new string[5, 5];

      for (var i = 0; i < 5; ++i) {
        for (var j = 0; j < 5; ++j) {
          returnMatrix[i, j] = this.GetCharacteristicInPosition(i, j);
        }
      }

      return returnMatrix;
    }


    /**
    * takes id of component in position, searches through all types and returns "field", "city" or "road" of component with searched id
    */
    public string GetCharacteristicInPosition(int i, int j) {
      var componentId = this.Matrix[i, j];

      foreach (var component in this.Types) {
        if (component.Id == componentId) {
          return component.Type;
        }
      }
      throw new Exception("Characteristic not found");
    }


    /**
    * searches for to search in each component type and replaces id
    * then replaces matrix ids
    */
    public void ChangeComponentIds(int toSearch, int toReplace) {
      foreach (var component in this.Types) {
        if (component.Id == toSearch) {
          component.Id = toReplace;
        }
      }

      for (var i = 0; i < this.Matrix.GetLength(0); ++i) {
        for (var j = 0; j < this.Matrix.GetLength(1); ++j) {
          if (this.Matrix[i, j] == toSearch) {
            this.Matrix[i, j] = toReplace;
          }
        }
      }
    }


    public override string ToString() {
      return "{\n" + $"\"name\": {this.Name}\n\"matrix\": \n{this.PrintMatrix()}\n\"types\": {this.PrintTypes()}" + "\n}";
    }


    public string PrintMatrix() {
      var x = new CustomArray<int>();
      return x.PrintArray(this.Matrix);
    }


    public string PrintTypes() {
      var returnString = "";
      foreach (var t in this.Types) {
        if (returnString == "") {
          returnString += "[\n" + t.ToString();
        } else {
          returnString += ", \n" + t.ToString();
        }
      }
      return returnString + "\n]";

    }
  }


  public class ComponentType {
    public int Id { get; set; }
    public string Type { get; set; }
    public List<int> Neighbors { get; set; }
    public List<float> Center { get; set; }


    public ComponentType(int id, string type, List<int> neighbors, List<float> center) {
      this.Id = id;
      this.Type = type;
      this.Neighbors = neighbors;
      this.Center = center;
    }


    public ComponentType(ComponentType another) {
      this.Id = another.Id;
      this.Type = another.Type;
      if (another.Neighbors == null) {
        this.Neighbors = null;
      } else {
        this.Neighbors = new List<int>(another.Neighbors);
      }
      this.Center = new List<float>(another.Center);
    }


    public ComponentType Clone() {
      return new ComponentType(this);
    }


    public override string ToString() {
      return $"{{\n\"id\": {this.Id}\n\"center\": {this.PrintList<float>(this.Center)}\n\"type\": {this.Type}\n\"neighbors\": {this.PrintList<int>(this.Neighbors)}\n}}";
    }


    public string PrintList<T>(List<T> l) {
      var x = new CustomArray<T>();
      return x.PrintList(l);
    }


  }


}