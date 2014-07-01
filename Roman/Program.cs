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
					new_text.Add(cur_string);
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
								cur_string ="  " + text[i];
								max_rang = j;
							}
						}
					}
				}



			}

			System.IO.File.WriteAllLines("new_text.txt",new_text);
			



		}
	}
}
