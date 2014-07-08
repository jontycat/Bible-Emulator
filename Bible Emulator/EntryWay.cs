using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

   public class EntryWay
    {
       private static Bible.BibleEnvironment mEnvironment = null;

       static void Main(string[] args)
       {
           Console.Title = "Bible Emulator";

           Console.ForegroundColor = ConsoleColor.Red;
           Console.WriteLine(@" ______  _ _     _       ");
           Console.WriteLine(@"(____  \(_) |   | |      ");
           Console.WriteLine(@" ____)  )_| | _ | | ____ ");
           Console.WriteLine(@"|  __  (| | || \| |/ _  )");
           Console.WriteLine(@"| |__)  ) | |_) ) ( (/ / ");
           Console.WriteLine(@"|______/|_|____/|_|\____)");
           Console.WriteLine(" ");
           Console.WriteLine("Prepare for revelations bitch");
           Console.WriteLine(" ");
           Console.WriteLine(" ");
           Console.ForegroundColor = ConsoleColor.White;
           setInstance(new Bible.BibleEnvironment());
           Console.ReadLine();
       }

        public static void setInstance(Bible.BibleEnvironment e)
        {
            if (e != null)
            mEnvironment = e;
        }

        public static Bible.BibleEnvironment returnEnvironment
        {
            get { return mEnvironment; }
        }
    }

