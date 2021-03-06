﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryManagmentVisualization
{
    class worstfit
    {
        public List<hole> holes;
        public List<process> processes;


        public List<process> holdProcesses = new List<process>(0);         //processes can't be allocated in memory
        public worstfit(List<hole> h, List<process> p)
        {
            holes = h;
            processes = p;
        }

        public void clearData()
        {
            holdProcesses.Clear();
        }
        public List<hole> copy(List<hole> t)
        {
            List<hole> w = new List<hole>();
            for (int i = 0; i < t.Count; i++) w.Add(t[i]);
            return w;
        }
        public void computeWorstFit()
        {
            while (processes.Count > 0)
            {
                process temp = processes[0];
                processes.RemoveAt(0);
                List<hole> tempHoles = new List<hole>();
                tempHoles = copy(holes);
                int allocated = 0;
                for (int i = 0; i < temp.no_of_segments; i++)
                {
                    hole.sort2(tempHoles);
                    tempHoles.Reverse();
                    for (int j = 0; j < tempHoles.Count; j++)
                    {
                        if (tempHoles[j].alocated == false && tempHoles[j].size >= temp.segmenst_sizes[i])
                        {
                            hole tempHole = new MemoryManagmentVisualization.hole(tempHoles[j].start, tempHoles[j].size);
                            tempHole.alocated = false;
                            tempHoles.RemoveAt(j);
                            int remainSize = tempHole.size - temp.segmenst_sizes[i];
                            tempHole.size = temp.segmenst_sizes[i];
                            allocated++;
                            tempHole.alocated = true;
                            tempHole.name = temp.name_of_segment[i];
                            tempHole.process_index = temp.process_id;
                            tempHoles.Add(tempHole);
                            if (remainSize > 0)
                            {
                                tempHoles.Add(new hole(tempHole.start + tempHole.size, remainSize));
                            }
                            break;

                        }
                    }
                }
                if (allocated == temp.no_of_segments)
                {
                    holes = copy(tempHoles);
                }
                else
                {
                    holdProcesses.Add(temp);
                }
            }
        }

        public void deallocator()
        {
            for (int i = 0; i < processes.Count; i++)
            {
                for (int j = 0; j < holes.Count; j++)
                {
                    if (holes[j].process_index == processes[i].process_id)
                    {
                        holes[j].alocated = false;
                        holes[j].name = "hole";
                        holes[j].process_index = -1;
                    }
                }
            }
            fix();
        }

        public void fix()
        {
            hole.sort(holes);
            List<hole> tempHoles = new List<hole>(0);
            hole tempBig = new MemoryManagmentVisualization.hole(0, 0);
            while (holes.Count > 0)
            {
                hole temp = holes[0];
                holes.RemoveAt(0);
                if (temp.alocated)
                {
                    tempHoles.Add(tempBig);
                    tempBig = new MemoryManagmentVisualization.hole(0, 0);
                    tempHoles.Add(temp);
                }
                else
                {
                    if (tempBig.size == 0)
                    {
                        tempBig = temp;
                    }
                    else if (temp.start == tempBig.start + tempBig.size)
                    {
                        tempBig.size = tempBig.size + temp.size;
                    }
                    else
                    {
                        tempHoles.Add(tempBig);
                        tempBig = temp;
                    }
                }
            }
            if (tempBig.size > 0) tempHoles.Add(tempBig);
            holes = tempHoles;
            hole.sort(holes);
            for (int i = 0; i < holes.Count; i++)
            {
                if (holes[i].size == 0)
                {
                    holes.RemoveAt(i); i--;
                }
            }
        }
        public void computebestFit()
        {
            while (processes.Count > 0)
            {
                process temp = processes[0];
                processes.RemoveAt(0);
                List<hole> tempHoles = new List<hole>();
                tempHoles = copy(holes);
                int allocated = 0;
                for (int i = 0; i < temp.no_of_segments; i++)
                {
                    hole.sort2(tempHoles);
                    
                    for (int j = 0; j < tempHoles.Count; j++)
                    {
                        if (tempHoles[j].alocated == false && tempHoles[j].size >= temp.segmenst_sizes[i])
                        {
                            hole tempHole = new MemoryManagmentVisualization.hole(tempHoles[j].start, tempHoles[j].size);
                            tempHole.alocated = false;
                            tempHoles.RemoveAt(j);
                            int remainSize = tempHole.size - temp.segmenst_sizes[i];
                            tempHole.size = temp.segmenst_sizes[i];
                            allocated++;
                            tempHole.alocated = true;
                            tempHole.name = temp.name_of_segment[i];
                            tempHole.process_index = temp.process_id;
                            tempHoles.Add(tempHole);
                            if (remainSize > 0)
                            {
                                tempHoles.Add(new hole(tempHole.start + tempHole.size, remainSize));
                            }
                            break;

                        }
                    }
                }
                if (allocated == temp.no_of_segments)
                {
                    holes = copy(tempHoles);
                }
                else
                {
                    holdProcesses.Add(temp);
                }
            }

        }
    }
}
