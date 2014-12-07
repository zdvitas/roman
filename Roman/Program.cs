using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Threading;


namespace Roman
{
	public class Event {
		public string data;
		public DateTime time;
		public string[] columns;

		public Event()
		{
		}
	}


	class MainClass
	{

		public static void Main (string[] args)
		{
			//make_new_text ();
			Func();
			//restrict_new_text ();

		}


		public static void Func(){
			string[] TB_text = System.IO.File.ReadAllLines("TB.txt");
			string[] Tibet_text = System.IO.File.ReadAllLines("Tibet1.txt");

			List<Event> Event_TB = new List<Event> ();
			List<Event> Event_Tibet = new List<Event> ();
			List<Event> rez_event = new List<Event> ();

			List<string> rez = new List<string> ();
		
			// Заносим ТВ в базу
			for (int i = 0; i < TB_text.Length; i++) {
				string[] tmp = TB_text [i].Split ('\t');

				// Преобразуем дату в читаьельный формат
				string date = tmp  [3] + "/" + tmp  [2] + "/" + tmp  [1] + " " +
					tmp  [4] + ":" + tmp [5] + ":" + tmp [6];
				// парсим дату
				try
				{
				DateTime.Parse (date);
				}
				catch {
					tmp[0] = "";
					//Console.WriteLine(date);
				}
				finally{
					if (tmp [0] != "") {
						Event t = new Event();
						t.data = TB_text [i];
						t.time = DateTime.Parse (date);
						t.columns = tmp;
						Event_TB.Add (t);
					}
					//Console.WriteLine(date);
				}
			}


			// Заносим тибет в базу

			for (int i = 0; i < Tibet_text.Length; i++) {
				var tmp = Tibet_text [i].Split ('\t');


				string date = tmp [3] + "/" + tmp [2] + "/" + tmp  [1] + " " +
					tmp  [4] + ":" + tmp [5] + ":" + tmp [6];


				Event t = new Event();
				t.data = Tibet_text [i];
				t.time = DateTime.Parse (date);
				t.columns = tmp;
				Event_Tibet.Add (t);
			}



			rez_event.AddRange (Event_TB);

		

			// Прихуячиваем второй список к первому
			int count_collisons = 0;   /// Эта переменная считает количество замещений
			bool flag = false;
			int index = 0;
			for (int i = 0; i < Event_Tibet.Count; i++) {
				Console.Clear ();
				Console.WriteLine ("Progress : "+i.ToString() + "/" + Event_Tibet.Count.ToString());
				Console.WriteLine ("Colision count : " + count_collisons.ToString());
				flag = true;

				// проверка событий
				for (int j = 0; j < Event_TB.Count; j++) {
					if (test_events (Event_TB[j] , Event_Tibet[i])) {
						// Значит что события произошли в одно время с заданными парамтерами
						index = j;    /// ИНдекс события из первого списка
						flag = false;
						break;

					}	

				}

				// или заменяем или добавляем
				if (flag) {

					// add
					Event_Tibet [i].data = "+++\t" + Event_Tibet[i].data; // Вот это закоментить
					rez_event.Add (Event_Tibet [i]);
			

				} else {
					// Сейчас будем заменять
					count_collisons ++;
				//	Console.WriteLine ("From: \t" + rez_event [index].data);
				//	Console.WriteLine (" To : \t" + Event_Tibet [i].data); // Пишет в консоль на что заменяем
				//	Console.WriteLine("--------------------------");
				//	Thread.Sleep (1000);
					Event_Tibet [i].data = "***\t" + Event_Tibet[i].data; // Вот это закоментить

					rez_event [index] = Event_Tibet [i];

				}





			}

			// Подготовка к печати в файл

			// Сортировка по времени 
			for (int i = 0; i < rez_event.Count; i ++)
			for (int j = 0; j< rez_event.Count-i -1; j++) {
				if (rez_event [j].time > rez_event [j + 1].time) {
					var tmp1 = rez_event [j];
					rez_event [j] = rez_event [j + 1];
					rez_event [j + 1] = tmp1;
					}
				}
			//

			for (int i = 0; i < rez_event.Count; i++) {
				rez.Add (rez_event [i].data);
			}


			System.IO.File.WriteAllLines("new_text2.txt",rez);
			Console.WriteLine("--------------------");
			Console.WriteLine ("Colissions count = " + count_collisons.ToString());
		}


		public static bool test_events(Event event1 , Event event2){

			float dt_float = 120f;

			var mag1 = float.Parse (event1.columns [7].Replace('.',','));
			var mag2 = float.Parse (event2.columns [7].Replace('.',','));
			var mag = Math.Abs(mag1 - mag2);
			float d_mag = 0.5f;

			var depth1 = float.Parse (event1.columns [12].Replace('.',','));
			var depth2 = float.Parse (event2.columns [12].Replace('.',','));
			var depth = Math.Abs( depth1 - depth2);
			float d_depth = 20.0f;


			var cord1_1 = float.Parse (event1.columns [10].Replace('.',','));
			var cord1_2 = float.Parse (event1.columns [11].Replace('.',','));

			var cord2_1 = float.Parse (event2.columns [10].Replace('.',','));
			var cord2_2 = float.Parse (event2.columns [11].Replace('.',','));

			var cord1 = Math.Abs (cord1_1 - cord2_1);
			var cord2 = Math.Abs (cord1_2 - cord2_2);
			float d_cord = 0.2f;


			TimeSpan dt;
			//return false; // НИкогда не заменяет
			if (event1.time > event2.time)
				dt = event1.time - event2.time;
			else
				dt = event2.time - event1.time;

			if (dt.TotalSeconds <= dt_float) {

				if (mag < d_mag) { // Магнитуда

					if (depth < d_depth) { // Гулибна
						if (cord1 < d_cord) {
							if (cord2 < d_cord) { // Координаты
								return true;
							}
						}
					}
				}
			}
			return false;
		}


	}


}
