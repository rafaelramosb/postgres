using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SkiChallenger
{
    class Route
    {
        public string Trace { get; set; }
        public int Station { get; set; }
        public int Elevetaion { get; set; }
    }

    public partial class _Default : System.Web.UI.Page
    {
        int row, col, countList=-1;
        int[,] Matrix;
        List<Route> traceList = new List<Route>();

        protected void Page_Load(object sender, EventArgs e)
        {
            loadMatrix();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    FindRoute(i, j, Matrix[i,j], true, -1);
                }
            }
            Route route = traceList.OrderBy(c => c.Station).ThenBy(c => c.Elevetaion).ElementAt(traceList.Count() - 1);//.Trace;
            
            Response.Write("<br>Length of calculated path: " + route.Station);
            Response.Write("<br>Drop of calculated path: " + route.Elevetaion);
            Response.Write("<br>Calculated path: " + route.Trace);
        }

        private void FindRoute(int i, int j, int elem, bool root, int listIndex)
        {
            bool endMatrix = false;
            bool intoRoute = false;

            if (checkMove(i, j, elem, 'N'))
            {
                endMatrix = true;
                if (root == true)
                {
                    traceList.Add(new Route { Trace = "", Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                traceList[listIndex].Trace = traceList[listIndex].Trace + Matrix[i, j].ToString() + "-";

                FindRoute(i - 1, j, Matrix[i - 1, j], false, listIndex);
                intoRoute = true;
            }

            if (checkMove(i, j, elem, 'S'))
            {
                endMatrix = true;
                if (root == true)
                {
                    traceList.Add(new Route { Trace = "", Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                if (intoRoute == true)
                {
                    string fork = traceList[listIndex].Trace.Split(new string[] { elem.ToString() + "-" }, StringSplitOptions.None)[0] + elem.ToString() + "-";

                    traceList.Add(new Route { Trace = fork, Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                else
                {
                    traceList[listIndex].Trace = traceList[listIndex].Trace + Matrix[i, j].ToString() + "-";
                }

                FindRoute(i + 1, j, Matrix[i + 1, j], false, listIndex);
                intoRoute = true;
            }

            if (checkMove(i, j, elem, 'E'))
            {
                endMatrix = true;
                if (root == true)
                {
                    traceList.Add(new Route { Trace = "", Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                if (intoRoute == true)
                {
                    string fork = traceList[listIndex].Trace.Split(new string[] { elem.ToString() + "-" }, StringSplitOptions.None)[0] + elem.ToString() + "-";

                    traceList.Add(new Route { Trace = fork, Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                else
                {
                    traceList[listIndex].Trace = traceList[listIndex].Trace + Matrix[i, j].ToString() + "-";
                }

                FindRoute(i, j + 1, Matrix[i, j + 1], false, listIndex);
                intoRoute = true;
            }

            if (checkMove(i, j, elem, 'W'))
            {
                endMatrix = true;
                if (root == true)
                {
                    traceList.Add(new Route { Trace = "", Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                if (intoRoute == true)
                {
                    string fork = traceList[listIndex].Trace.Split(new string[] { elem.ToString() + "-" }, StringSplitOptions.None)[0] + elem.ToString() + "-";

                    traceList.Add(new Route { Trace = fork, Station = 0, Elevetaion = 0 });
                    ++countList;
                    listIndex = countList;
                }
                else
                {
                    traceList[listIndex].Trace = traceList[listIndex].Trace + Matrix[i, j].ToString() + "-";
                }

                FindRoute(i, j - 1, Matrix[i, j - 1], false, listIndex);
            }

            if (root == false && endMatrix == false)
            {
                traceList[listIndex].Trace = traceList[listIndex].Trace + Matrix[i, j].ToString() + "-";
                
                if (traceList[listIndex].Trace.Length > 1)
                {
                    List<string> tempList = traceList[listIndex].Trace.Split('-').ToList();
                    tempList.RemoveAt(tempList.Count - 1);
                    traceList[listIndex].Station = tempList.Count;

                    traceList[listIndex].Elevetaion = Int32.Parse(tempList[0].ToString()) - Int32.Parse(tempList[tempList.Count - 1].ToString());
                }
            }
        }

        private void loadMatrix()
        {
            string dataFileInput = File.ReadAllText(Server.MapPath("~/App_Data/map.txt"));
            //string dataFileInput = File.ReadAllText(Server.MapPath("~/App_Data/4x4.txt"));
            //List<string> linesFileInput = dataFileInput.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
            List<string> linesFileInput = dataFileInput.Split(new string[] { "\n" }, StringSplitOptions.None).ToList<string>();
            row = Int32.Parse(linesFileInput.ElementAt(0).Split(' ')[0]);
            col = Int32.Parse(linesFileInput.ElementAt(0).Split(' ')[1]);

            Matrix = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                List<string> digits = linesFileInput.ElementAt(i + 1).Split(' ').ToList<string>();
                for (int j = 0; j < col; j++)
                {
                    Matrix[i, j] = Int32.Parse(digits.ElementAt(j));
                }
            }
        }

        private bool checkMove(int i, int j, int elem, char direction)
        {
            switch (direction)
            {
                case 'N':
                    {
                        if (i == 0)
                            return false;

                        if (elem > Matrix[i - 1, j])
                            return true;
                        else
                            return false;
                    }
                case 'S':
                    {
                        if (i == row-1)
                            return false;

                        if (elem > Matrix[i + 1, j])
                            return true;
                        else
                            return false;
                    }
                case 'E':
                    {
                        if (j == col - 1)
                            return false;

                        if (elem > Matrix[i, j + 1])
                            return true;
                        else
                            return false;
                    }
                case 'W':
                    {
                        if (j == 0)
                            return false;

                        if (elem > Matrix[i, j - 1])
                            return true;
                        else
                            return false;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
