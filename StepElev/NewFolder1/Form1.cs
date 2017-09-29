using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StepElev.NewFolder1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked=true;
            textBox1.Enabled = false;
            textBox2.Enabled= false;
            int rise = 6;
            textBox1.Text =rise.ToString();
            int offset = 6;
            textBox2.Text = offset.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {                    
            this.Hide(); // this is not mandatory
            if (radioButton1.Checked)
            {
                int Foffset = int.Parse(textBox2.Text);
                int rise = int.Parse(textBox1.Text);
                string sLayerName = "1 ALL-Elevation Medium";
                Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();
                // do your stuff here                    
                foundationSteps(Foffset, sLayerName);
                
            }
            else
                if (radioButton2.Checked)
            {
                int Foffset = int.Parse(textBox2.Text);
                int rise = int.Parse(textBox1.Text);
                string sLayerName = "3-WD-Flashing";
                Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();
                foundationSteps(Foffset, sLayerName);
            }
            }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //textBox1.Clear();
            //textBox1.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           // toolTip1.SetToolTip(textBox1, "Rise should be between 3-14");
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            int offset = 6;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox2.Text = offset.ToString();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            int rise = 6;
            textBox1.Text = rise.ToString();
            int offset = 4;
            textBox2.Text = offset.ToString();
        }
        public static Point3d FirstPoint(string message)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            PromptPointResult res;
            PromptPointOptions opts = new PromptPointOptions("");
            opts.Message = message;
            res = acDoc.Editor.GetPoint(opts);
            Point3d fpoint = res.Value;

            if (res.Status == PromptStatus.OK)
                return fpoint;
            else
                return fpoint;

        }
        public static Point3d SecondPoint(string message, Point3d point)
        {
           
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;
            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Orthomode = false;
            PromptPointResult res;
            PromptPointOptions opts = new PromptPointOptions("");
            opts.UseBasePoint = true;
            opts.BasePoint = point;

            opts.Message = message;
            res = acDoc.Editor.GetPoint(opts);
            Point3d spoint = res.Value;
            if (res.Status == PromptStatus.OK)
                return spoint;
            else
                return spoint;

        }
        public static void foundationSteps(double off, string sLayerName)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            try {
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    LayerTable acLyrTbl;
                    acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;
                    LayerTableRecord acLyrTblRec;
                    // string sLayerName = "1 ALL-Elevation Medium";
                    if (!acLyrTbl.Has(sLayerName))
                    {
                        acLyrTblRec = new LayerTableRecord();

                        // Assign the layer a name
                        acLyrTblRec.Name = sLayerName;

                        // Upgrade the Layer table for write
                        acLyrTbl.UpgradeOpen();

                        // Append the new layer to the Layer table and the transaction
                        acLyrTbl.Add(acLyrTblRec);
                        acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);

                    }
                    else
                    {
                        acLyrTblRec = acTrans.GetObject(acLyrTbl[sLayerName],
                                                        OpenMode.ForRead) as LayerTableRecord;
                    }
                    // acCurDb.Clayer = acLyrTbl[sLayerName];
                    //acLyrTblRec.IsOff = true;

                    BlockTable acBlkTbl;
                    BlockTableRecord acBlkTblRec;

                    // Open Model space for write
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                    OpenMode.ForRead) as BlockTable;

                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;
                    Point3d start = FirstPoint("\nPlease enter first point");
                    Point3d end = SecondPoint("\nPlease enter second point", start);
                    double slope = Calulate_Slope(start, end);
                    if (slope >= 6)
                    {
                        //using (Line acLine = new Line(start, end))
                        //{
                        //    // Add the line to the drawing
                        //    acLine.SetDatabaseDefaults();
                        //    acBlkTblRec.AppendEntity(acLine);
                        //    acTrans.AddNewlyCreatedDBObject(acLine, true);
                        //    acLine.Erase();
                        //}
                        double offDeltaX = deltaX(start, end, off);
                        double offDeltaY = deltaY(start, end, off);
                        Point3d ptDelta = new Point3d(-offDeltaY, offDeltaX, 0);
                        Point3d pt = new Point3d(start.X + ptDelta.X, start.Y + ptDelta.Y, 0);
                        Point3d npt = new Point3d(end.X + ptDelta.X, end.Y + ptDelta.Y, 0);

                        //using (Line nLine = new Line(start, pt))
                        //{
                        //    // Add the line to the drawing
                        //    nLine.SetDatabaseDefaults();
                        //    nLine.ColorIndex = 6;
                        //    acBlkTblRec.AppendEntity(nLine);
                        //    acTrans.AddNewlyCreatedDBObject(nLine, true);
                        //    nLine.Erase();

                        //}
                        //using (Line nLine = new Line(pt, npt))
                        //{
                        //    // Add the line to the drawing
                        //    nLine.SetDatabaseDefaults();
                        //    acBlkTblRec.AppendEntity(nLine);
                        //    nLine.ColorIndex = 1;
                        //    acTrans.AddNewlyCreatedDBObject(nLine, true);
                        //    nLine.Erase();
                        //}
                        double x = pt.Y;
                        double y = npt.Y;
                        double rise = 6;
                        Point3d rp = new Point3d(pt.X, pt.Y + rise, pt.Z);

                        //using (Line nLine = new Line(pt, rp))
                        //{
                        //    // Add the line to the drawing
                        //    nLine.SetDatabaseDefaults();
                        //    nLine.ColorIndex = 4;
                        //    acBlkTblRec.AppendEntity(nLine);
                        //    acTrans.AddNewlyCreatedDBObject(nLine, true);
                        //    nLine.Erase();
                        //}

                        // double rpdeltaX = rise * Math.Tan(angle);

                        double rpdeltax = rpdeltaX(start, end);
                        Point3dCollection ptcol = new Point3dCollection();

                        Point3d tempPoint = new Point3d(pt.X, pt.Y, pt.Z);
                        ptcol.Add(pt);

                        for (double i = x; i < y;)
                        {
                            Point3d firstPoint = new Point3d(tempPoint.X, tempPoint.Y + rise, 0);
                            Point3d secondPoint = new Point3d(firstPoint.X + rpdeltax, firstPoint.Y, 0);
                            ptcol.Add(new Point3d(firstPoint.X, firstPoint.Y, 0));
                            ptcol.Add(new Point3d(secondPoint.X, secondPoint.Y, 0));
                            tempPoint = secondPoint;
                            i = i + rise;
                        }

                        Polyline acPoly = new Polyline();
                        acPoly.SetDatabaseDefaults();

                        for (int i = 0; i < ptcol.Count; i++)
                        {
                            acPoly.AddVertexAt(i, new Point2d(ptcol[i].X, ptcol[i].Y), 0, 0, 0);
                        }
                        //acPoly.ColorIndex = 1;
                        acPoly.Layer = sLayerName;
                        acBlkTblRec.AppendEntity(acPoly);
                        acTrans.AddNewlyCreatedDBObject(acPoly, true);

                    }
                    else
                    {
                        Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Slope Too shallow,Steps are not required");
                    }


                    acTrans.Commit();
                }
               
            }
            catch (Autodesk.AutoCAD.Runtime.Exception Ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("The following exception was caught:\n" +
                                            Ex.Message);
            }

        }
        public static double rpdeltaX(Point3d pStart, Point3d pEnd)
        {
            double Xdiff = Math.Abs(pEnd.X - pStart.X);
            double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
            double angle = Math.Atan(Xdiff / Ydiff);
            double rise = 6;
            double rpdeltaX = rise * Math.Tan(angle);
            return rpdeltaX;
        }
        public static double deltaX(Point3d pStart, Point3d pEnd, double off)
        {
            double Xdiff = Math.Abs(pEnd.X - pStart.X);
            double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
            double angle = Math.Atan(Xdiff / Ydiff);
            double offset = off;
            double deltax = offset * Math.Sin(angle);
            return deltax;
        }
        public static double deltaY(Point3d pStart, Point3d pEnd, double off)
        {
            double Xdiff = Math.Abs(pEnd.X - pStart.X);
            double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
            double angle = Math.Atan(Xdiff / Ydiff);
            double offset = off;
            double deltay = offset * Math.Cos(angle);
            return deltay;
        }
        public static double Calulate_Slope(Point3d pStart, Point3d pEnd)
        {
            double Xdiff = Math.Abs(pEnd.X - pStart.X);
            double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
            return Ydiff;

        }

    }
}
