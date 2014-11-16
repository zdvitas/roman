using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;


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
			string[] rang_1 = {"ave","ccc","chi","den","dze","hod"};
			string[] rang_2 = {"hrv","mcd","mgk","mos","nis","mostr"};
			string[] rang_3 = {"oth","riz","rud","sip","yng","sip","rudik1","rudik2"};
		

			List<string[]> rangs = new List<string[]>();
			rangs.Add(rang_1);
			rangs.Add(rang_2);
			rangs.Add(rang_3);


			string[] text = System.IO.File.ReadAllLines("CATALOG.COL");

			for(int j = 0; j< text.Length; j++){
				for(int i = 0; i < 5; i++)
				{
					text[j] = text[j].Replace("  "," " );
				}
				if(text[j][0] == ' ')
					text[j] = text[j].Remove(0,1);
			}

			
			List<string> new_text = new List<string>();

			new_text.Add(text[0]);

			string cur_string="";
			int max_rang = int.MaxValue;

			for(int i=1; i< text.Length; i++)
			{
				var tmp = text[i].Split(' ');
				if(tmp.Length == 12)
				{

					new_text[new_text.Count-1] +=" " + cur_string ;
					cur_string = "";
					new_text.Add(text[i]);
					max_rang = int.MaxValue;
				} else
				{
					string status = tmp[6].Split('-')[0];
					status = status.Split('_')[0];

					for(int j = 0; j < rangs.Count; j++)
					{
						if (Array.IndexOf(rangs[j] , status) >= 0)
						{
							if(j<max_rang)
							{
								cur_string =text[i];
								cur_string.Remove(0,1);
								max_rang = j;

							}
						}
					}
				}



			} 


			System.IO.File.WriteAllLines("new_text.txt",new_text);
			//Func();



		}


		public static void Func(){



			string[] text1 = System.IO.File.ReadAllLines("new_text.txt");
			string[] text2 = System.IO.File.ReadAllLines("Tibet1.txt");

			List<string> str_rez = new List<string> ();

			List<Event> EventList1 = new List<Event> ();
			List<Event> EventList2 = new List<Event> ();
			List<Event> rez_event = new List<Event> ();

			List<string> rez = new List<string> ();
		
			// Костыль на кривую дату
			for (int i = 0; i < text1.Length; i++) {
				string[] tmp = text1 [i].Split (' ');

				if (tmp  [2].Length == 1) {
					tmp  [2] = "190" + tmp  [2];
				} else
					tmp  [2] = "19" + tmp  [2];

				if (tmp  [5] == "-1" || tmp  [5]=="24") {
					tmp  [5] = "0"; 
				}

				
				if (tmp  [6] == "-1") {
					tmp  [6] = "0"; 
				}

				
				if (tmp  [7] == "-1") {
					tmp [7] = "0"; 
				}

				// Преобразуем дату в читаьельный формат
				string date = tmp  [4] + "/" + tmp  [3] + "/" + tmp  [2] + " " +
					tmp  [5] + ":" + tmp [6] + ":" + tmp [7];
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
						t.data = text1 [i];
						t.time = DateTime.Parse (date);
						t.columns = tmp;
						EventList1.Add (t);
					}
					//Console.WriteLine(date);
				}
			}



			//
			List<DateTime> Times2 = new List<DateTime> ();
			for (int i = 0; i < text2.Length; i++) {
				text2[i] = text2 [i].Replace ('\t' ,' ');
				var tmp = text2 [i].Split (' ');


				string date = tmp [3] + "/" + tmp [2] + "/" + tmp  [1] + " " +
					tmp  [4] + ":" + tmp [5] + ":" + tmp [6];


				Event t = new Event();
				t.data = text2 [i];
				t.time = DateTime.Parse (date);
				t.columns = tmp;
				EventList2.Add (t);
			}



			rez_event.AddRange (EventList1);

		
			// Это тупое прихуячивание
			//rez.AddRange (list2);

			// Прихуячиваем второй список к первому
			int count_collisons = 0;   /// Эта переменная считает количество замещений
			bool flag = false;
			int index = 0;
			for (int i = 0; i < EventList2.Count; i++) {
				Console.Clear ();
				Console.Write ("Progress : "+i.ToString() + "/" + EventList2.Count.ToString());
				flag = true;
				for (int j = 0; j < EventList1.Count; j++) {
					if (test_events (EventList1[j] , EventList2[i])) {
						// Значит что события произошли в одно время
						index = j;    /// ИНдекс события из первого списка
						flag = false;
						break;

					}	

				}


				if (flag) {
					rez_event.Add (EventList2 [i]);
	

				} else {
					// Сейчас будем заменять
					count_collisons ++;
					Console.WriteLine ("From: \t" + rez_event [index].data);
					Console.WriteLine (" To : \t" + EventList2 [i].data); // Пишет в консоль на что заменяем
					Console.WriteLine("--------------------------");
					rez_event [index] = EventList2 [i];

				}





			}



			// Сортировка по времени 
			for (int i = 0; i < rez_event.Count; i ++)
			for (int j = 0; j< rez_event.Count-i -1; j++) {
				if (rez_event [j].time > rez_event [j + 1].time) {
					var tmp1 = rez_event [j];
					rez_event [j] = rez_event [j + 1];
					rez_event [j + 1] = tmp1;
					}
				}

			for (int i = 0; i < rez_event.Count; i++) {
				rez.Add (rez_event [i].data);
			}


			System.IO.File.WriteAllLines("new_text2.txt",rez);
			Console.WriteLine (count_collisons);
		}


		public static bool test_events(Event event1 , Event event2){

			var mag1 = float.Parse (event1.columns [8].Replace('.',','));
			var mag2 = float.Parse (event2.columns [7].Replace('.',','));
			float d_mag = 0.5f;
			var depth1 = float.Parse (event1.columns [9].Replace('.',','));
			var depth2 = float.Parse (event2.columns [8].Replace('.',','));
			float d_depth = 20.0f;
			var cord1_1 = float.Parse (event1.columns [10].Replace('.',','));
			var cord1_2 = float.Parse (event1.columns [11].Replace('.',','));

			var cord2_1 = float.Parse (event1.columns [9].Replace('.',','));
			var cord2_2 = float.Parse (event1.columns [10].Replace('.',','));
			float d_cord = 0.2f;

			TimeSpan dt;
			//return false; // НИкогда не заменяет
			if (event1.time > event2.time)
				dt = event1.time - event2.time;
			else
				dt = event2.time - event1.time;

			if (dt.TotalSeconds <= 3)

			if (Math.Abs (mag1 - mag2) < d_mag) // Магнитуда
			if (Math.Abs (depth1 - depth2) < d_depth) // Гулибна
			if ((Math.Abs (cord1_1 - cord2_1) < d_cord) && (Math.Abs (cord1_2 - cord2_2) < d_cord)) // Координаты
				return true;

			return false;
		}


	}


}
