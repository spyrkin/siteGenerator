﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using live.Entity;
using live.Entity.Base;

namespace live.Stages
{
    public class Stage4: Stage
    {
        public override void WORK()
        {
            Console.WriteLine("");


            //создание TRAVEL
            createTravels();


            //создание обычных моделей
            foreach (var rf in PATH.refs)
            {
                createModels(rf.destinationPass);
            }


        }

        public void createTravels()
        {
            DirectoryInfo di = new DirectoryInfo(PATH.datat);
            string _type = di.Name;
            DirectoryInfo[] diA = di.GetDirectories();

            foreach (DirectoryInfo f in diA)
            {
                createTravel(f);
            }
            int j = DATA._TRAVELS.Count();
            string ins = new String(' ', 20 - "TRAVEL".Length);
            Console.WriteLine(String.Format("{0}{1} {2} {4} {3}", CONST._INS, "MODELS: ", "TRAVEL", j, ins));

        }

        public void createTravel(DirectoryInfo f)
        {
            TRAVEL travel = new TRAVEL();
            string name = f.Name;
            travel.dataFolderPath = f.FullName;

            travel.Id = Convert.ToInt32(name);
            string content = FILEWORK.ReadFileContent(f.FullName + "//info.txt");
            String[] lines = content.Split(new string[] { "\n" }, StringSplitOptions.None);


            #region parse order.txt
            string order = FILEWORK.ReadFileContent(f.FullName + "//order.txt");
            String[] order_lines = order.Split(new string[] { "\n" }, StringSplitOptions.None);
            travel.order = order_lines.ToList();
            #endregion

            #region youtubs.txt

            if (File.Exists(f.FullName + "//youtubs.txt"))
            {
                string ycontent = FILEWORK.ReadFileContent(f.FullName + "//youtubs.txt");
                String[] ycontent_lines = ycontent.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (var you in ycontent_lines)
                {
                    string syou = parseYoutubs(you);
                    travel.youtubs.Add(syou);
                }
            }
            #endregion



            travel.description = FILEWORK.ReadFileContent(f.FullName + "//description.txt");

            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (travel.name == null)
                {
                    travel.name = line;
                    continue;
                }

                if (travel.ldate == null)
                {
                    travel.ldate = line;
                    continue;
                }

                if (travel.lcount == null)
                {
                    travel.lcount = line;
                    continue;
                }

                if (line.Contains(".jpg"))
                {
                    travel.mainIng.Add(line);
                    continue;

                }

                if (travel.praceS == null)
                {
                    travel.praceS = line;
                    continue;
                }



                if (travel.praceL == null)
                {
                    travel.praceL = line;
                    continue;
                }




            }



            List<string> imgs = new List<string>();
            travel.imgs = FILEWORK.GetAllFiles(travel.dataFolderPath, imgs, ".jpg");

            List<string> txts = new List<string>();
            txts = FILEWORK.GetAllFiles(travel.dataFolderPath, txts, ".txt");
            foreach (string s in txts)
            {
                FileInfo ff = new FileInfo(s);
                string fname = ff.Name;
                String[] ll = fname.Split(new string[] { ".txt" }, StringSplitOptions.None);
                string shortName = ll[0];
                bool success = travel.order.Contains(shortName);
                if (success)
                {

                    addLSText(shortName, travel, ff.FullName);


                }
            }

            DATA._TRAVELS.Add(travel);
        }

        private string parseYoutubs(string line)
        {
            string result = "";
            String[] ll = line.Split(new string[] { "/" }, StringSplitOptions.None);
            string last = ll.Last();
            result = "https://www.youtube.com/embed/" + last;
            return result;
        }

        private void addLSText(string number, TRAVEL travel, string ffFullName)
        {
            string l;
            string s;
            List<string> ll = new List<string>();
            List<string> ls = new List<string>();
            string stage = "";
            string content = FILEWORK.ReadFileContent(ffFullName);
            String[] lines = content.Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.ToUpper() == "LENA")
                {
                    stage = "lena";
                    continue;
                }

                if (line.ToUpper() == "SERGEY")
                {
                    stage = "sergey";
                    continue;
                }

                if (stage == "lena")
                {
                    ll.Add(line);
                }

                if (stage == "sergey")
                {
                    ls.Add(line);
                }
            }

            l = string.Join("\n", ll.ToArray());
            s = string.Join("\n", ls.ToArray());
            TRAVEL.LSTEXT tt = new TRAVEL.LSTEXT();
            tt.l = l;
            tt.s = s;
            tt.day = number;


            travel.destrictions.Add(number, tt);
        }


        private void createModels(string dirPathFull)
        {
            int j = 0;
            DirectoryInfo di = new DirectoryInfo(dirPathFull);
            string _type = di.Name;
            DirectoryInfo[] diA = di.GetDirectories();
            foreach (var f in diA)
            {
                string path = f.FullName;
                CONTENT content = getContent(path, _type);
                j++;

                switch (_type)
                {
                    case "DOGANDCAT":
                        content.link = "http://kapybara.ru/data/dogandcat/" + content.Id.ToString() + ".html";
                        content._type = "DOGANDCAT";
                        DATA._DOGANDCAT.Add(content as DOGANDCAT);
                        DATA._CONTENT.Add(content);

                        break;
                    case "FRIENDS":
                        content.link = "http://kapybara.ru/data/friends/" + content.Id.ToString() + ".html";
                        content._type = "FRIENDS";
                        DATA._FRIENDS.Add(content as FRIENDS);
                        DATA._CONTENT.Add(content);

                        break;
                    case "SPORT":
                        content.link = "http://kapybara.ru/data/sport/" + content.Id.ToString() + ".html";
                        content._type = "SPORT";
                        DATA._SPORT.Add(content as SPORT);
                        DATA._CONTENT.Add(content);

                        break;
                    case "WORKOUT":
                        content.link = "http://kapybara.ru/data/workout/" + content.Id.ToString() + ".html";
                        content._type = "WORKOUT";

                        DATA._WORKOUT.Add(content as WORKOUT);
                        DATA._CONTENT.Add(content);

                        break;
                    case "FOOD":
                        content.link = "http://kapybara.ru/data/food/" + content.Id.ToString() + ".html";
                        content._type = "FOOD";

                        DATA._FOOD.Add(content as FOOD);
                        DATA._CONTENT.Add(content);

                        break;


                    case "BOOK":
                        content.link = "http://kapybara.ru/data/book/" + content.Id.ToString() + ".html";
                        content._type = "BOOK";
                        DATA._BOOK.Add(content as BOOK);
                        DATA._CONTENT.Add(content);
                        break;

                    default:
                        Console.WriteLine("WRONG TYPE!!!");
                        break;
                }

            }
            DATA._DOGANDCAT = DATA._DOGANDCAT.OrderBy(o => o.Id).ToList();
            DATA._FRIENDS = DATA._FRIENDS.OrderBy(o => o.Id).ToList();
            DATA._SPORT = DATA._SPORT.OrderBy(o => o.Id).ToList();
            DATA._WORKOUT = DATA._WORKOUT.OrderBy(o => o.Id).ToList();
            DATA._FOOD = DATA._FOOD.OrderBy(o => o.Id).ToList();


            string ins = new String(' ', 20 - _type.Length);

            Console.WriteLine(String.Format("{0}{1} {2} {4} {3}", CONST._INS, "MODELS: ", _type, j,ins));



        }

        private CONTENT getContent(String path, string _type)
        {
            CONTENT content = new CONTENT();


            switch (_type)
            {
                case "DOGANDCAT":
                    content = new DOGANDCAT();
                    break;
                case "FRIENDS":
                    content = new FRIENDS();
                    break;
                case "SPORT":
                    content = new SPORT();
                    break;
                case "WORKOUT":
                    content = new WORKOUT();
                    break;
                case "FOOD":
                    content = new FOOD();
                    break;
                case "BOOK":
                    content = new BOOK();
                    break;
                default:
                    Console.WriteLine("WRONG TYPE!!!");
                    break;
            }

            String[] ll = path.Split(new string[] {"\\"}, StringSplitOptions.None);
            string si = ll.Last();
            content.Id = Convert.ToInt32(si);
            content.dataFolderPath = path;
            content.parse();
            return content;
        }

        //заполняемые данные


        //Создание моделей
        public Stage4(string name) : base(name)
        {

        }
    }
}
