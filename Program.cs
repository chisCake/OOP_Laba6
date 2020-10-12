using System;
using System.Collections.Generic;

namespace OOP_Laba6 {
	class Program {
		delegate void Delegate();
		static readonly List<Delegate> Tasks = new List<Delegate> {
			new Delegate(Print),
			new Delegate(Add),
			new Delegate(Remove),
			new Delegate(Search),
			new Delegate(Count),
			new Delegate(PrintSortByPrice)
		};

		static Laboratory lab = new Laboratory(
				new Technique(TechniqueType.Computer, (2015, 3, 15), 200),
				new Technique(TechniqueType.Computer, (2018, 8, 20), 400),
				new Technique(TechniqueType.Microscop, (2020, 3, 2), 120),
				new Technique(TechniqueType.Microscop, (2018, 11, 16), 100),
				new Technique(TechniqueType.Microscop, (2018, 9, 28), 180),
				new Technique(TechniqueType.Refrigerator, (2012, 1, 14), 600),
				new Technique(TechniqueType.Refrigerator, (2018, 8, 29), 700),
				new Technique(TechniqueType.Centrifuge, (2019, 3, 15), 650),
				new Technique(TechniqueType.Centrifuge, (2014, 7, 24), 900),
				new Technique(TechniqueType.Thermometer, (2020, 5, 18), 170)
			);

		static void Main() {
			try {
				var temp = Controller.ReadJson();
				lab = temp;
			}
			catch (Exception err) {
				Console.WriteLine(err.Message);
				Console.ReadKey();
			}
			//lab = DB.ReadTxt();
			while (true) {
				Console.Clear();
				Console.Write(
					"1 - вывести список техники" +
					"\n2 - добавить новую технику" +
					"\n3 - удалить технику из списка" +
					"\n4 - найти технику по сроку службы" +
					"\n5 - подсчитать кол-во техники каждого вида" +
					"\n6 - вывести список техники в порядке убывания цены" +
					"\n0 - выход" +
					"\nВыберите действие: "
					);

				if (!int.TryParse(Console.ReadLine(), out int choice) || (choice > Tasks.Count || choice < 0)) {
					Console.WriteLine("Нет такого действия");
					Console.ReadKey();
					continue;
				}

				if (choice == 0)
					return;

				Tasks[choice - 1]();
				Console.ReadKey();
			}
		}

		static void Print() {
			lab.PrintContent();
		}

		static void Add() {
			Console.Write(
				"1 - компьютер" +
				"\n2 - микроскоп" +
				"\n3 - холодильник" +
				"\n4 - центрифуга" +
				"\n5 - термометр" +
				"\nВыберите тип техники: ");

			try {
				if (!int.TryParse(Console.ReadLine(), out int choice) || (choice < 1 || choice > 5)) {
					throw new Exception("Неверно выбран тип");
				}

				TechniqueType type = (TechniqueType)choice;
				Console.Write("Введите дату поставки в формате дд.мм.гггг: ");
				if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
					throw new Exception("Неверно введена дата");

				Console.Write("Введите цену: ");
				if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
					throw new Exception("Неверно введена цена");

				lab.Add(new Technique(type, date, price));
				Controller.Write(lab);
				Console.WriteLine("Элемент добавлен");
			}
			catch (Exception err) {
				Console.WriteLine("Ошибка в вводе данных: " + err.Message);
				return;
			}

		}

		static void Remove() {
			lab.PrintContent();
			Console.Write("Введите номер для удаления: ");
			if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > lab.Components.Count) {
				Console.WriteLine("Нет такого номера");
				return;
			}
			if (lab.Remove(choice - 1))
				Console.WriteLine("Элемент удалён");
			else
				Console.WriteLine("Элемент не найден");
			Controller.Write(lab);
		}

		static void Search() {
			Console.Write("Введите дату дд.мм.гггг: ");
			if (!DateTime.TryParse(Console.ReadLine(), out DateTime date)) {
				Console.WriteLine("Нерверно введено значение");
				Console.ReadKey();
				return;
			}
			var list = Controller.GetByDate(lab, date);
			Console.WriteLine(" №  Тип техники    Дата появления    Цена");
			int counter = 0;
			foreach (var item in list) {
				Console.WriteLine($"{++counter,2}) {item.GetType(),-13}    {item.ReceiptDate:d}    {item.Price,5}р");
			}
		}

		static void Count() {
			var dict = lab.CountEachType();
			Console.WriteLine("Тип             кол-во");
			foreach (var item in dict) {
				Console.WriteLine($"{Technique.GetStrType(item.Key),-12} {item.Value,7}");
			}
		}

		static void PrintSortByPrice() {
			Controller.PrintSortedByPrice(lab);
		}
	}
}
