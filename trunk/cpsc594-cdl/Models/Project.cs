using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpsc594_cdl.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        List<Component> Components;
        List<Iteration> TotalIterations;

        public Project(int ID, string Name)
        {
            this.ProjectID = ID;
            this.Name = Name;
        }

        public void setComponents(List<Component> Components, IEnumerable<int> ComponentIDs)
        {
            int iterationIndex;
            int[] linesExecutedList;
            int[] linesCoveredList;

            // Setup TotalIterations
            TotalIterations = new List<Iteration>();
            List<Iteration> IterationList = Components.FirstOrDefault().Iterations;
            foreach (Iteration iteration in IterationList)
            {
                TotalIterations.Add(iteration.clone());
            }
            linesExecutedList = new int[TotalIterations.Count()];
            linesCoveredList = new int[TotalIterations.Count()];
            foreach (Component component in Components)
            {
                iterationIndex = 0;
                foreach (Iteration currIteration in component.Iterations)
                {
                    linesExecutedList[iterationIndex] += currIteration.coverage.GetValue();
                    linesCoveredList[iterationIndex] += currIteration.coverage.GetLinesCovered();
                    iterationIndex++;
                }
            }
            iterationIndex = 0;
            foreach (Iteration iteration in TotalIterations)
            {
                TotalIterations.ElementAt(iterationIndex).coverage
                    = new CoverageMetric(-1, -1, linesExecutedList[iterationIndex], linesCoveredList[iterationIndex], DateTime.UtcNow);
                iterationIndex++;
            }

            // Setup Components
            this.Components = new List<Component>();
            foreach (Component component in Components)
                if (ComponentIDs.Contains(component.ComponentID))
                    this.Components.Add(component);
        }

        public List<Component> GetComponents()
        {
            return Components;
        }

        public List<Iteration> GetTotalIterations()
        {
            return TotalIterations;
        }
    }
}