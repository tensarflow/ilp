using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ILP
{
    class Robot
    {
        private RoboDK.Item ROBOT = null;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void initialize()
        {
            //Logger.updateLogFile("\r\nInitializing robot!", false);

            string robot_ip = "192.168.1.2";
            RoboDK RDK = new RoboDK(start_hidden: true);

            string filename = "C:/RoboDK/Library/UR3.robot";
            RoboDK.Item item = RDK.AddFile(filename);
            

            ROBOT = RDK.getItem("UR3");

            if (ROBOT.Valid())
            {
                Console.WriteLine("Using robot: " + ROBOT.Name());
            }
            else
            {
                Console.WriteLine("Robot not available.");
            }

            // Connect to real robot
            if (ROBOT.Connect(robot_ip))
            {
                // Set to Run on Robot robot mode:
                RDK.setRunMode(RoboDK.RUNMODE_RUN_ROBOT);
                Console.WriteLine("Connected to Robot.");
                //Logger.updateLogFile("\r\nnInitializing robot done!", false);
            }
            else
            {
                Console.WriteLine("Can't connect to the robot. Check connection and parameters.");
                MessageBox.Show("Can't connect to the robot. Check connection and parameters.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void moveSample()
        {

            //Logger.updateLogFile("\r\nMove Started!", false);

            // Move to Home
            Console.WriteLine("Moving to home");
            ROBOT.setSpeed(200);
            double[] joints_home = ROBOT.JointsHome();
            ROBOT.MoveJ(joints_home, false);

            // Generate new positions based on current positon home -> rectangele
            Mat poseHomeM = ROBOT.Pose();
            double[] home = poseHomeM.ToTxyzRxyz();

            double[] newPos_1 = home;
            newPos_1[0] = home[0] + 100;
            Mat newPosM_1 = Mat.FromTxyzRxyz(newPos_1);

            double[] newPos_2 = newPos_1;
            newPos_2[1] = newPos_1[1] + 100;
            Mat newPosM_2 = Mat.FromTxyzRxyz(newPos_2);

            double[] newPos_3 = newPos_2;
            newPos_3[0] = newPos_2[0] - 100;
            Mat newPosM_3 = Mat.FromTxyzRxyz(newPos_3);

            // Move Robot
            Console.WriteLine("Moving to newPos_1");
            ROBOT.MoveL(newPosM_1, false);

            Console.WriteLine("Moving to newPos_2");
            ROBOT.MoveL(newPosM_2, false);

            Console.WriteLine("Moving to newPos_3");
            ROBOT.MoveL(newPosM_3, false);

            Console.WriteLine("Moving to home");
            ROBOT.MoveL(poseHomeM, false);

            //Logger.updateLogFile("\r\nMove Ended", false);
        }
    }
}
