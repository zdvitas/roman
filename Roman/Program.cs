using System;
using System.Collections.Generic;
using System.IO;



namespace Roman
{
	class MainClass
	{
	

		public static void Main (string[] args)
		{
			string[] rang_1 = {"ave","ccc","chi","den","dze","hod"};
			string[] rang_2 = {"hrv","mcd","mgk","mos","nis"};
			string[] rang_3 = {"oth","riz","rud","sip","yng"};
		

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
			Func();



		}


		public static void Func(){
			DateTime time;
			time = DateTime.Parse("01/03/2008 07:00:00");
			Console.Write (time.ToString ());
			string[] text1 = System.IO.File.ReadAllLines("new_text.txt");
			string[] text2 = System.IO.File.ReadAllLines("Tibet1.txt");

			List<string> str_rez = new List<string> ();


			List<string> list1 = new List<string> ();
			List<string> list2 = new List<string> ();
			List<string> rez = new List<string> ();
			List<DateTime> Times = new List<DateTime> ();

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


				string date = tmp  [4] + "/" + tmp  [3] + "/" + tmp  [2] + " " +
					tmp  [5] + ":" + tmp [6] + ":" + tmp [7];
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




			for (int i = 0; i < text2.Length; i++) {
				var tmp = text2 [i].Split ('\t');
				list2.Add (text2[i]);

				string date = tmp [3] + "/" + tmp [2] + "/" + tmp  [1] + " " +
					tmp  [4] + ":" + tmp [5] + ":" + tmp [6];

				Times.Add (DateTime.Parse (date));

			}


			rez.AddRange (list1);
			rez.AddRange (list2);



			for (int i = 0; i < Times.Count; i ++)
				for (int j = 0; j< Times.Count-i -1; j++) {
					if (Times [j] > Times [j + 1]) {
						var tmp1 = Times [j];
						Times [j] = Times [j + 1];
						Times [j + 1] = tmp1;

						var tmp2 = rez [j];
						rez [j] = rez [j + 1];
						rez [j + 1] = tmp2;
					}
				}



			System.IO.File.WriteAllLines("new_text2.txt",rez);

		}


	}
}
