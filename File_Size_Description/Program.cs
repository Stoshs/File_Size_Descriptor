using System;
using System.Collections;
using System.IO;
using System.Text;

namespace File_Size_Description
{
    class Program
    {
        static StringBuilder sb = new StringBuilder();
        static StringBuilder sb_hundred_below = new StringBuilder();
        static StringBuilder sb_hundred_mb = new StringBuilder();
        static StringBuilder sb_one_gb = new StringBuilder();
        static StringBuilder sb_ten_gb = new StringBuilder();

        // main method.
        static void Main(string[] args)
        {
            display_starting_information();
            String start_dir = get_starting_directory();
            recursive_file_loop(start_dir);

            //String txt_file = get_txt_file_name(); // full path of txt file
            String txt_file = get_txt_file_name_documents(); // name of txt file stored in my_documents

            write_txt_file(txt_file);
        }

        // display information about the program and ask for starting directory
        private static void display_starting_information()
        {
            Console.WriteLine("Show all files and their respective file size.");
            Console.WriteLine("");
            Console.Write("Enter the starting directory: ");
        }

        // get the starting directory from the user
        private static String get_starting_directory()
        {
            String starting_dir = Console.ReadLine();
            Console.WriteLine("Starting directory is: " + starting_dir);
            return starting_dir;
        }

        private static void recursive_file_loop(String starting_directory)
        {
            String[] files;
            String[] directories;
            int bytes_constraint = 100000000; // 100 MB
            int one_gb = 1000000000; // 1 GB
            double ten_gb = 10000000000; // 10 GB

            try
            {
                files = Directory.GetFiles(starting_directory);

                foreach (String file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Length > ten_gb)
                    {
                        sb.AppendLine(fi.FullName);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(file + " | " + fi.Length, Console.ForegroundColor);
                        Console.ForegroundColor = ConsoleColor.White;
                        sb_ten_gb.AppendLine("\t" + convert_bytes_to_gb(fi.Length) + " GBs | " + fi.FullName);
                    }
                    else if (fi.Length > one_gb)
                    {
                        sb.AppendLine(fi.FullName);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(file + " | " + fi.Length, Console.ForegroundColor);
                        Console.ForegroundColor = ConsoleColor.White;
                        sb_one_gb.AppendLine("\t" + convert_bytes_to_gb(fi.Length) + " GBs | " + fi.FullName);
                    }
                    else if (fi.Length > bytes_constraint)
                    {
                        sb.AppendLine(fi.FullName);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(file + " | " + fi.Length, Console.ForegroundColor);
                        Console.ForegroundColor = ConsoleColor.White;
                        sb_hundred_mb.AppendLine("\t" + convert_bytes_to_mb(fi.Length) + " MBs | " + fi.FullName);
                    }
                    else
                    {
                        Console.WriteLine(file + " | " + fi.Length);
                        sb_hundred_below.AppendLine("\t" + convert_bytes_to_mb(fi.Length) + " MBs | " + fi.FullName);
                    }
                }


                directories = Directory.GetDirectories(starting_directory);

                foreach (String sub_dir in directories)
                {
                    recursive_file_loop(sub_dir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error looping through files");
            }
        }

        // get full path to save data to a txt file
        private static String get_txt_file_name()
        {
            String txt_file = "";

            Console.Write("Enter the full path of the txt file: ");
            txt_file = Console.ReadLine();

            return txt_file;
        }

        // get name of txt file to save in documents
        private static String get_txt_file_name_documents()
        {
            String txt_file = "";

            Console.Write("Enter the name of the txt file: ");
            txt_file = Console.ReadLine();
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";

            return path + txt_file + ".txt";
        }

        // convert file bytes to GBs
        private static double convert_bytes_to_gb(long bytes)
        {
            double gbs = 0;
            double tmp = bytes / 1000000000.00;

            gbs = Math.Round(tmp, 3);

            return gbs;
        }

        // convert file bytes to MBs
        private static double convert_bytes_to_mb(long bytes)
        {
            double mbs = 0;
            double tmp = bytes / 1000000.00;

            mbs = Math.Round(tmp, 3);

            return mbs;
        }

        private static void write_txt_file(String file_path)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file_path))
            {
                // write 10 GB files or larger
                writer.WriteLine("10 GB or larger: ");
                writer.WriteLine(sb_ten_gb.ToString());
                writer.WriteLine("");

                // write 1GB - 10GB files
                writer.WriteLine("1 GB to 10 GB: ");
                writer.WriteLine(sb_one_gb.ToString());
                writer.WriteLine("");

                // write 100MB - 1 GB files
                writer.WriteLine("100 MB to 1 GB: ");
                writer.WriteLine(sb_hundred_mb.ToString());
                writer.WriteLine("");

                // write 100MB files or less
                writer.WriteLine("100 MBs or less: ");
                writer.WriteLine(sb_hundred_below.ToString());
                writer.WriteLine();
            }
        }
    }
}
