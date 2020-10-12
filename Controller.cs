using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace OOP_Laba6 {
	static class Controller {
		public static Laboratory GetSortedByPrice(Laboratory laboratory) {
			var list = new List<Technique>();
			foreach (var item in laboratory.Components)
				list.Add((Technique)item);
			list.Sort((item1, item2) => item2.Price.CompareTo(item1.Price));
			var result = new Laboratory(list.ToArray());
			return result;
		}

		public static void PrintSortedByPrice(Laboratory laboratory) {
			var sorted = GetSortedByPrice(laboratory);
			sorted.PrintContent(sort: false);
		}

		public static List<Technique> GetByDate(Laboratory laboratory, DateTime date) {
			var list = new List<Technique>();
			foreach (var item in laboratory.Components)
				if (((Technique)item).ReceiptDate < date)
					list.Add((Technique)item);
			return list;
		}

		public static Dictionary<TechniqueType, int> CountEachType(this Laboratory laboratory) {
			var dict = new Dictionary<TechniqueType, int>{
				{TechniqueType.Centrifuge, 0},
				{TechniqueType.Computer, 0},
				{TechniqueType.Microscop, 0},
				{TechniqueType.Refrigerator, 0},
				{TechniqueType.Thermometer, 0}
			};
			foreach (var item in laboratory.Components) {
				switch (((Technique)item).Type) {
					case TechniqueType.Centrifuge:
						dict[TechniqueType.Centrifuge]++;
						break;
					case TechniqueType.Computer:
						dict[TechniqueType.Computer]++;
						break;
					case TechniqueType.Microscop:
						dict[TechniqueType.Microscop]++;
						break;
					case TechniqueType.Refrigerator:
						dict[TechniqueType.Refrigerator]++;
						break;
					case TechniqueType.Thermometer:
						dict[TechniqueType.Thermometer]++;
						break;
					default:
						throw new Exception("Не распознан тип объекта");
				}
			}
			return dict;
		}

		public static void Write(Laboratory laboratory) {
			using var sw = new StreamWriter("lab.json");
			var list = new List<Technique>();
			foreach (var item in laboratory.Components)
				list.Add((Technique)item);
			string json = JsonConvert.SerializeObject(list);
			sw.WriteLine(json);
		}

		public static Laboratory ReadJson() {
			if (!File.Exists("lab.json"))
				throw new Exception("File not found");
			using var sr = new StreamReader("lab.json");
			string json = sr.ReadToEnd();
			var list = JsonConvert.DeserializeObject<List<Technique>>(json);
			var laboratory = new Laboratory();
			foreach (var item in list)
				laboratory.Add(item);
			return laboratory;
		}

		public static Laboratory ReadTxt() {
			if (!File.Exists("lab.txt"))
				throw new Exception("File not found");
			using var sr = new StreamReader("lab.txt");
			var laboratory = new Laboratory();
			string line;
			while ((line = sr.ReadLine()) != null) {
				var items = line.Split(';');
				laboratory.Add(new Technique(Technique.GetEnumType(items[0]), DateTime.Parse(items[1]), Convert.ToInt32(items[2])));
			}
			return laboratory;
		}
	}
}
