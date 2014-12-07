/*
public static string mas_to_string(string[] mas)
{
	// 0	1	  2	  3 4  5  6  7    8    9       10    11    12   13   14   15   16   17   18
	// N    Nver Год  М Д Час М Сек  Mg   Lon      Lat  Depth  Ver  AzP  PlP  AzT PlT  str   NAME
	string delimeter = "\t";
	string rez = "";
	// Убираем 2 лишниее колонки и записываем дату и время
	for (int i = 2; i < 8; i++) {
		rez += mas [i] + delimeter;
	} 
	//в) "MHL","E" это две одинаковые колонки в нашем списке это Mg
	rez += mas[8] + delimeter;
	rez += mas[8] + delimeter;
	//г) "kl" у нас такой колонки нет, поэтому зафигачиваем ее единицами всю
	rez += "1" + delimeter ;
	//д) "Ld","FI"  = нашим "Lon","Lat"
	rez += mas[9] + delimeter;
	rez += mas[10] + delimeter;
	// е) "Depth" = Depth
	rez += mas[11] + delimeter;
	// ж) "Pazm","Ppl","Tazm","Tpl" = нашим  AzP  PlP  AzT PlT
	rez += mas[13] + delimeter;
	rez += mas[14] + delimeter;
	rez += mas[15] + delimeter;
	rez += mas[16] + delimeter;
	//з)  "D" = str 	и)  "N" = Name 	к)  "B" = Nver	л)  "C" = Ver
	rez += mas[17] + delimeter;
	rez += mas[18] + delimeter;
	rez += mas[1] + delimeter;
	rez += mas[12] + delimeter;
	return rez;
}

public static void restrict_new_text()
{
	string[] text = System.IO.File.ReadAllLines("new_text.txt");
	List<string> new_text2 = new List<string> ();
	for (int i = 0; i < text.Length; i++) {
		string[] tmp = text [i].Split (' ');

		if (tmp [2].Length == 1) {
			tmp [2] = "190" + tmp [2];
		} else
			tmp [2] = "19" + tmp [2];

		if (tmp [5] == "-1" || tmp [5] == "24") {
			tmp [5] = "0"; 
		}


		if (tmp [6] == "-1") {
			tmp [6] = "0"; 
		}


		if (tmp [7] == "-1") {
			tmp [7] = "0"; 
		}

		new_text2.Add (mas_to_string (tmp));

	}
	System.IO.File.WriteAllLines("new_text2.txt",new_text2);
}

public static void make_new_text()
{
	//			string[] rang_1 = {"ave","ccc","chi","den","dze","hod"};
	//			string[] rang_2 = {"hrv","mcd","mgk","mos","nis","mostr"};
	//			string[] rang_3 = {"oth","riz","rud","sip","yng","sip","rudik1","rudik2"};

	string[] rang_1 = { "hrv", "cmt", "dze", "mcd" };
	string[] rang_2 = { "hod" }; 
	string[] rang_3 = { "sip" };
	string[] rang_4 = { "ccc" };
	string[] rang_5 = { "mgk" };
	string[] rang_6 = { "rudik1", "rudik2" };
	string[] rang_7 = { "riz" };
	string[] rang_8 = { "nis" };
	string[] rang_9 = { "den", "ave" };
	string[] rang_10 = { "yng" };
	string[] rang_11 = { "mostr", "chi", "oth" };



	List<string[]> rangs = new List<string[]>();
	rangs.Add(rang_1);
	rangs.Add(rang_2);
	rangs.Add(rang_3);
	rangs.Add(rang_4);
	rangs.Add(rang_5);
	rangs.Add(rang_6);
	rangs.Add(rang_7);
	rangs.Add(rang_8);
	rangs.Add(rang_9);
	rangs.Add(rang_10);
	rangs.Add(rang_11);


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

	//for(int i=1; i< 300; i++)
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
			string status = tmp[6].Replace('_','-');
			status = status.Split('-')[0];
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
	new_text[new_text.Count-1] +=" " + cur_string ;
	System.IO.File.WriteAllLines("new_text.txt",new_text);
}
*/