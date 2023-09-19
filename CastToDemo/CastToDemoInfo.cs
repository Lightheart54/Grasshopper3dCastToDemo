using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace CastToDemo
{
    public class CastToDemoInfo : GH_AssemblyInfo
    {
        public override string Name => "CastToDemo";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("04c9bfab-b21e-41ca-8f7a-009618473235");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}