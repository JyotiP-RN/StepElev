// (C) Copyright 2017 by HP Inc. 
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(StepElev.MyCommands))]

namespace StepElev
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        [CommandMethod("ElevStep",CommandFlags.Modal)]
        public static void showform()
        {
            StepElev.NewFolder1.Form1 frm = new NewFolder1.Form1();
            Application.ShowModalDialog(frm);
            frm.Dispose();
        }
        //public static void stepElev()
        //{
        //    Application.DocumentManager.MdiActiveDocument.Database.Orthomode = false;
        //    Document acDoc = Application.DocumentManager.MdiActiveDocument;
        //    Database acCurDb = acDoc.Database;
        //    using (acDoc.LockDocument())
        //    { 
        //            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");
        //            pKeyOpts.Message = "\nChoose Your option ";
        //            pKeyOpts.Keywords.Add("Foundation");
        //            pKeyOpts.Keywords.Add("Roof");
                  
        //           // pKeyOpts.Keywords.Default = "Foundation";
        //            pKeyOpts.AllowNone = true;

        //            PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);
        //            string s = pKeyRes.StringResult;
        //           // Application.ShowAlertDialog("Entered keyword: " +
        //                                     //pKeyRes.StringResult);
        //        string sLayerName;
                
        //            switch (s)
        //            {
        //                case "Foundation":
        //                    double Foffset =6;
        //                   sLayerName= "1 ALL-Elevation Medium";
        //                foundationSteps(Foffset,sLayerName);
        //                    break;
        //                case "Roof":
        //                    double Roffset = 4;
        //                sLayerName="3-WD-Flashing";
        //                foundationSteps(Roffset,sLayerName);
        //                    break;
                           
        //            }
                
        //    }
        //}
        
        //public static Point3d FirstPoint(string message)
        //{
        //    Document acDoc = Application.DocumentManager.MdiActiveDocument;
        //    Database acCurDb = acDoc.Database;
            
        //        PromptPointResult res;
        //        PromptPointOptions opts = new PromptPointOptions("");
        //         opts.Message = message;
        //        res = acDoc.Editor.GetPoint(opts);
        //        Point3d fpoint = res.Value;             
        
        //       if (res.Status == PromptStatus.OK)
        //        return fpoint;
        //      else
        //        return fpoint;        
           
        //}
        //public static Point3d SecondPoint(string message, Point3d point)
        //{
        //    Document acDoc = Application.DocumentManager.MdiActiveDocument;
        //    Database acCurDb = acDoc.Database;
           
        //    PromptPointResult res;
        //    PromptPointOptions opts = new PromptPointOptions("");
        //    opts.UseBasePoint = true;
        //    opts.BasePoint = point;

        //    opts.Message = message;
        //    res = acDoc.Editor.GetPoint(opts);
        //    Point3d spoint = res.Value;
        //    if (res.Status == PromptStatus.OK)
        //        return spoint;
        //    else
        //        return spoint;
              
        //}     


        //public static void foundationSteps(double off,string sLayerName)
        //{
        //    Document acDoc = Application.DocumentManager.MdiActiveDocument;
        //    Database acCurDb = acDoc.Database;
            
        //    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
        //    {   LayerTable acLyrTbl;
        //        acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;
        //        LayerTableRecord acLyrTblRec;               
        //       // string sLayerName = "1 ALL-Elevation Medium";
        //        if (!acLyrTbl.Has(sLayerName))
        //        {
        //            acLyrTblRec = new LayerTableRecord();

        //            // Assign the layer a name
        //            acLyrTblRec.Name = sLayerName;

        //            // Upgrade the Layer table for write
        //            acLyrTbl.UpgradeOpen();

        //            // Append the new layer to the Layer table and the transaction
        //            acLyrTbl.Add(acLyrTblRec);
        //            acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);

        //        }
        //        else
        //        {
        //            acLyrTblRec = acTrans.GetObject(acLyrTbl[sLayerName],
        //                                            OpenMode.ForRead) as LayerTableRecord;
        //        }
        //       // acCurDb.Clayer = acLyrTbl[sLayerName];
        //        //acLyrTblRec.IsOff = true;
                
        //        BlockTable acBlkTbl;
        //        BlockTableRecord acBlkTblRec;

        //        // Open Model space for write
        //        acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
        //                                        OpenMode.ForRead) as BlockTable;

        //        acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
        //                                        OpenMode.ForWrite) as BlockTableRecord;
        //        Point3d start = FirstPoint("\nPlease enter first point");
        //        Point3d end = SecondPoint("\nPlease enter second point", start);
        //        double slope = Calulate_Slope(start, end);
        //        if (slope>=6)
        //        {
        //            //using (Line acLine = new Line(start, end))
        //            //{
        //            //    // Add the line to the drawing
        //            //    acLine.SetDatabaseDefaults();
        //            //    acBlkTblRec.AppendEntity(acLine);
        //            //    acTrans.AddNewlyCreatedDBObject(acLine, true);
        //            //    acLine.Erase();
        //            //}
        //            double offDeltaX = deltaX(start,end,off);
        //            double offDeltaY = deltaY(start, end,off);
        //            Point3d ptDelta = new Point3d(-offDeltaY, offDeltaX, 0);
        //            Point3d pt = new Point3d(start.X + ptDelta.X, start.Y + ptDelta.Y, 0);
        //            Point3d npt = new Point3d(end.X + ptDelta.X, end.Y + ptDelta.Y, 0);

        //            //using (Line nLine = new Line(start, pt))
        //            //{
        //            //    // Add the line to the drawing
        //            //    nLine.SetDatabaseDefaults();
        //            //    nLine.ColorIndex = 6;
        //            //    acBlkTblRec.AppendEntity(nLine);
        //            //    acTrans.AddNewlyCreatedDBObject(nLine, true);
        //            //    nLine.Erase();

        //            //}
        //            //using (Line nLine = new Line(pt, npt))
        //            //{
        //            //    // Add the line to the drawing
        //            //    nLine.SetDatabaseDefaults();
        //            //    acBlkTblRec.AppendEntity(nLine);
        //            //    nLine.ColorIndex = 1;
        //            //    acTrans.AddNewlyCreatedDBObject(nLine, true);
        //            //    nLine.Erase();
        //            //}
        //            double x = pt.Y;
        //            double y = npt.Y;
        //            double rise = 6;
        //            Point3d rp = new Point3d(pt.X, pt.Y + rise, pt.Z);

        //            //using (Line nLine = new Line(pt, rp))
        //            //{
        //            //    // Add the line to the drawing
        //            //    nLine.SetDatabaseDefaults();
        //            //    nLine.ColorIndex = 4;
        //            //    acBlkTblRec.AppendEntity(nLine);
        //            //    acTrans.AddNewlyCreatedDBObject(nLine, true);
        //            //    nLine.Erase();
        //            //}

        //            // double rpdeltaX = rise * Math.Tan(angle);

        //            double rpdeltax = rpdeltaX(start,end);
        //            Point3dCollection ptcol = new Point3dCollection();

        //            Point3d tempPoint = new Point3d(pt.X, pt.Y, pt.Z);
        //            ptcol.Add(pt);

        //            for (double i = x; i < y;)
        //            {
        //                Point3d firstPoint = new Point3d(tempPoint.X, tempPoint.Y + rise, 0);
        //                Point3d secondPoint = new Point3d(firstPoint.X + rpdeltax, firstPoint.Y, 0);
        //                ptcol.Add(new Point3d(firstPoint.X, firstPoint.Y, 0));
        //                ptcol.Add(new Point3d(secondPoint.X, secondPoint.Y, 0));
        //                tempPoint = secondPoint;
        //                i = i + rise;
        //            }

        //            Polyline acPoly = new Polyline();
        //            acPoly.SetDatabaseDefaults();
                    
        //            for (int i = 0; i < ptcol.Count; i++)
        //            {
        //                acPoly.AddVertexAt(i, new Point2d(ptcol[i].X, ptcol[i].Y), 0, 0, 0);
        //            }
        //            //acPoly.ColorIndex = 1;
        //            acPoly.Layer = sLayerName;
        //            acBlkTblRec.AppendEntity(acPoly);
        //            acTrans.AddNewlyCreatedDBObject(acPoly, true);
                    
        //        }
        //        else
        //        {
        //            Application.ShowAlertDialog("Slope Too shallow,Steps are not required");
        //        }
        //        acTrans.Commit();
        //    }
        //}
        //public static double rpdeltaX(Point3d pStart,Point3d pEnd)
        //{
        //    double Xdiff = Math.Abs(pEnd.X - pStart.X);
        //    double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
        //    double angle = Math.Atan(Xdiff / Ydiff);
        //    double rise = 6;
        //    double rpdeltaX = rise * Math.Tan(angle);
        //    return rpdeltaX;
        //}
        //public static double deltaX(Point3d pStart,Point3d pEnd,double off)
        //{
        //    double Xdiff = Math.Abs(pEnd.X - pStart.X);
        //    double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
        //    double angle = Math.Atan(Xdiff / Ydiff);
        //    double offset = off;
        //    double deltax = offset * Math.Sin(angle);
        //    return deltax;
        //}
        //public static double deltaY(Point3d pStart, Point3d pEnd,double off)
        //{
        //    double Xdiff = Math.Abs(pEnd.X - pStart.X);
        //    double Ydiff = Math.Abs(pEnd.Y - pStart.Y);
        //    double angle = Math.Atan(Xdiff / Ydiff);
        //    double offset =off;
        //    double deltay = offset * Math.Cos(angle);
        //    return deltay;
        //}
        //public static double Calulate_Slope(Point3d pStart,Point3d pEnd)
        //{
        //    double Xdiff = Math.Abs(pEnd.X - pStart.X);
        //    double Ydiff =Math.Abs( pEnd.Y - pStart.Y);            
        //    return Ydiff;          

        //}
        
      
    }

}
