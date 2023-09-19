using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace CastToDemo
{
    public class CastToDemoComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CastToDemoComponent()
          : base("CastToDemoComponent", "CastTo",
            "Demonstrates Weird Behavior when Casting to Geometry Base",
            "Demo", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "Brep", "brep representing a trimmed surface", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("BrepCastResult", "BrepResult", "Result of CastTo Geometry Base from a GH_Brep", GH_ParamAccess.item);
            pManager.AddGeometryParameter("SurfaceCastResult", "SurfaceResult", "Result of CastTo Geometry Base from a GH_Surface", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Brep baseData = new GH_Brep();
            if (!DA.GetData<GH_Brep>(0, ref baseData)) return;

            
            Brep brepData = baseData.Value.DuplicateBrep();
            if (brepData.Surfaces.Count > 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Must pass a single trimmed surface into the node");
                return;
            }

            //Create a new GH_Brep and GH_Surface from the underlying data
            /* This is some more weird behavior, the compiler won't let me cast to GeometryBase from
             * the concrete implementations, but is just fine if I do it from the interface
             * The error message is an unhelpful:
             * error CS1503: Argument 1: cannot convert from 'out Rhino.Geometry.GeometryBase' to 'out Rhino.Geometry.GeometryBase'
             */
            IGH_GeometricGoo newGHBrep = new GH_Brep(brepData);
            IGH_GeometricGoo newGHSurface = new GH_Surface(brepData);

            //preform castTo<GeometryBase>
            if(!newGHBrep.CastTo<GeometryBase>(out var brepBase))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed Brep Cast To");
                return;
            }
            if(!newGHSurface.CastTo<GeometryBase>(out var surfaceBase))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed Surface Cast To");
                return;
            }

            if(brepBase.ObjectType != surfaceBase.ObjectType)
            {
                var errMsg = $@"Casting to GeometryBase changed the underlying data type of the data,
Brep Cast Type Result: {brepBase.ObjectType}
Surface Cast Type Result: {surfaceBase.ObjectType}";
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error,errMsg);
            }

            //Pass the data back out
            DA.SetData(0, brepBase);
            DA.SetData(1, surfaceBase);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("dd590d4d-eaa5-429d-bc84-0b015a92f8bf");
    }
}