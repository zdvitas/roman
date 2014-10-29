using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;


namespace Roman
{
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

					new_text[new_text.Count-1] +=cur_string ;
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


			//System.IO.File.WriteAllLines("new_text.txt",new_text);
			Func();



		}


		public static void Func(){



			string[] text1 = System.IO.File.ReadAllLines("new_text.txt");
			string[] text2 = System.IO.File.ReadAllLines("Tibet1.txt");

			List<string> str_rez = new List<string> ();


			List<string> list1 = new List<string> ();
			List<string> list2 = new List<string> ();
			List<string> rez = new List<string> ();
			List<DateTime> Times = new List<DateTime> ();


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
				Times.Add (DateTime.Parse (date));
				}
				catch {
					tmp[0] = "";
					//Console.WriteLine(date);
				}
				finally{
					if (tmp [0] != "") {
						text1 [i].Replace (' ', '\t');
						list1.Add (text1 [i]);
					}
					//Console.WriteLine(date);
				}
			}



			//
			List<DateTime> Times2 = new List<DateTime> ();
			for (int i = 0; i < text2.Length; i++) {
				text2[i] = text2 [i].Replace ('\t' ,' ');
				var tmp = text2 [i].Split (' ');
				list2.Add (text2[i]);

				string date = tmp [3] + "/" + tmp [2] + "/" + tmp  [1] + " " +
					tmp  [4] + ":" + tmp [5] + ":" + tmp [6];
				Times2.Add (DateTime.Parse (date));

			}


			rez.AddRange (list1);
			List<DateTime> rezTime = new List<DateTime> ();
			rezTime.AddRange (Times);
			// Это тупое прихуячивание
			//rez.AddRange (list2);

			// Прихуячиваем второй список к первому
			int count_collisons = 0;   /// Эта переменная считает количество замещений
			bool flag = false;
			int index = 0;
			for (int i = 0; i < Times2.Count; i++) {
				flag = true;
				for (int j = 0; j < Times.Count; j++) {
					if (test_events (text1 [j], text2 [i], Times [j], Times2 [i])) {
						// Значит что события произошли в одно время
						index = j;    /// ИНдекс события из первого списка
						flag = false;
						break;

					}	

				}


				if (flag) {

					rez.Add (text2 [i]);
					rezTime.Add (Times2 [i]);
		

				} else {
					// Сейчас будем заменять
					count_collisons ++;
					Console.WriteLine ("From: \t" + rez [index]);
					Console.WriteLine (" To : \t" + text2 [i]); // Пишет в консоль на что заменяем
					Console.WriteLine("--------------------------");
					rez [index] = text2 [i];
					Times [index] = Times2 [i];
				}





			}



			// Сортировка по времени 
			for (int i = 0; i < rezTime.Count; i ++)
			for (int j = 0; j< rezTime.Count-i -1; j++) {
				if (rezTime [j] > rezTime [j + 1]) {
					var tmp1 = rezTime [j];
					rezTime [j] = rezTime [j + 1];
					rezTime [j + 1] = tmp1;

						var tmp2 = rez [j];
						rez [j] = rez [j + 1];
						rez [j + 1] = tmp2;
					}
				}



			System.IO.File.WriteAllLines("new_text2.txt",rez);
			Console.WriteLine (count_collisons);
		}
		
		public static bool test_events(string event1 , string event2 , DateTime time1 , DateTime time2){
			var event1_colums = event1.Split (' ');
			var event2_colums = event2.Split (' ');
			var mag1 = float.Parse (event1_colums [8], CultureInfo.InvariantCulture);
			var mag2 = float.Parse (event2_colums [7], CultureInfo.InvariantCulture);
			float d_mag = 0.5f;
			var depth1 = float.Parse (event1_colums [9], CultureInfo.InvariantCulture);
			var depth2 = float.Parse (event2_colums [8], CultureInfo.InvariantCulture);
			float d_depth = 20.0f;
			var cord1_1 = float.Parse (event1_colums [10], CultureInfo.InvariantCulture);
			var cord1_2 = float.Parse (event1_colums [11], CultureInfo.InvariantCulture);

			var cord2_1 = float.Parse (event1_colums [9], CultureInfo.InvariantCulture);
			var cord2_2 = float.Parse (event1_colums [10], CultureInfo.InvariantCulture);
			float d_cord = 0.2f;

			TimeSpan dt;
			//return false; // НИкогда не заменяет
			if (time1 > time2)
				dt = time1 - time2;
			else
				dt = time2 - time1;

			if (dt.TotalSeconds <= 3)

			if (Math.Abs (mag1 - mag2) < d_mag) // Магнитуда
			if (Math.Abs (depth1 - depth2) < d_depth) // Гулибна
			if ((Math.Abs (cord1_1 - cord2_1) < d_cord) && (Math.Abs (cord1_2 - cord2_2) < d_cord))
				return true;

			return false;
		}


	}


}
