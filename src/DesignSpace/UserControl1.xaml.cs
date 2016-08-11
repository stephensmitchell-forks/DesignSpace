﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DesignSpace
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        /// <summary>
        /// Main contructor
        /// TODO: Needs to have GH_Mesh as argument
        /// </summary>
        public UserControl1(Mesh myRhinoMesh)
        {
            InitializeComponent();
            Create3DViewPort(myRhinoMesh);
        }

        /// <summary>
        /// Create the 3d viewport
        /// </summary>
        private void Create3DViewPort(Mesh myRhinoMesh)
        {
            var hVp3D = new HelixViewport3D();
            hVp3D.Background = Brushes.DarkGray;

            hVp3D.ShowFrameRate = true;
            hVp3D.ViewCubeOpacity = 0.2;
            hVp3D.ViewCubeTopText = "T";
            hVp3D.ViewCubeFrontText = "R";
            hVp3D.ViewCubeRightText= "";
            hVp3D.ViewCubeBottomText = "";
            hVp3D.ViewCubeLeftText = "F";
            hVp3D.ViewCubeBackText = "";

            var lights = new DefaultLights();
            var teaPot = new Teapot();
            hVp3D.Children.Add(lights);
            //hVp3D.Children.Add(teaPot);

            /*
             * Whenever you can, use Visual3D objects for unique instances of objects within your scene. 
             * This usage contrasts with that of Model3D objects, which are lightweight objects that are optimized to be shared and reused. 
             * For example, use a Model3Dobject to build a model of a car; and use ten ModelVisual3D objects to place ten cars in your scene.
             */

            MeshGeometry3D myMesh = new MeshGeometry3D();
            Point3DCollection myPoints = new Point3DCollection();

            if (myRhinoMesh != null)
            {
                for (int i = 0; i < myRhinoMesh.Vertices.Count; i++)
                {
                    myPoints.Add(new Point3D(myRhinoMesh.Vertices[i].X, myRhinoMesh.Vertices[i].Y, myRhinoMesh.Vertices[i].Z));
                }

                myMesh.Positions = myPoints;


                for (int i = 0; i < myRhinoMesh.Faces.Count; i++)
                {
                    myMesh.TriangleIndices.Add(myRhinoMesh.Faces[i].A);
                    myMesh.TriangleIndices.Add(myRhinoMesh.Faces[i].B);
                    myMesh.TriangleIndices.Add(myRhinoMesh.Faces[i].C);
                }
            }

            // Define material that will use the gradient.
            // DiffuseMaterial myDiffuseMaterial = new DiffuseMaterial(Brushes.Black);
            // Add this gradient to a MaterialGroup.
            // MaterialGroup myMaterialGroup = new MaterialGroup();
            // myMaterialGroup.Children.Add(myDiffuseMaterial);

            DiffuseMaterial wireframe_material = new DiffuseMaterial(Brushes.White);
            GeometryModel3D WireframeModel = new GeometryModel3D(myMesh, wireframe_material);

            ModelVisual3D monkey = new ModelVisual3D();
            monkey.Content = WireframeModel;

            hVp3D.Children.Add(monkey);

            hVp3D.ZoomExtents();


            //hVp3D.IsEnabled = false;

            this.AddChild(hVp3D);


            
        }
    }
}
