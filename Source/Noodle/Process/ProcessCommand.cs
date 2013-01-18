﻿using System;
using System.Diagnostics;

namespace Noodle.Process
{
    public class ProcessCommand
    {
        public Func<ProcessStartInfo> ProcessStartBuilder = null;
        public Action<ProcessStartInfo> ProcessStartModifier = null;
        public EventHandler<DataReceivedEventArgs> OnError;
        public EventHandler<DataReceivedEventArgs> OnOutput;

        public ProcessCommand()
        {
            Timeout = 5000;
        }

        public ProcessCommand(string fileName)
        {
            ProcessStartBuilder = () => new ProcessStartInfo(fileName);
            Timeout = 5000;
        }

        public ProcessCommand(string fileName, string arguments)
        {
            ProcessStartBuilder = () => new ProcessStartInfo(fileName,arguments);
            Timeout = 5000;
        }

        public int Timeout { get; set; }

        public InvocationResult Invoke()
        {
            if (ProcessStartBuilder == null)
                throw new InvalidOperationException("You must specifiy a ProcessStartBuilder or don't use an empty contructer");

            var process = ProcessStartBuilder();

            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            process.RedirectStandardError = true;
            process.RedirectStandardInput = true;
            process.RedirectStandardOutput = true;
            process.WindowStyle = ProcessWindowStyle.Hidden;

            if (ProcessStartModifier != null)
                ProcessStartModifier(process);

            var result = new InvocationResult();

            try
            {
                var proc = new System.Diagnostics.Process();
                proc.ErrorDataReceived += (sender, e) =>
                                              {
                                                  if(OnError != null)
                                                  {
                                                      OnError(sender, e);
                                                  }
                                              };

                proc.OutputDataReceived += (sender, e) =>
                {
                    if (OnOutput != null)
                    {
                        OnOutput(sender, e);
                    }
                };
                proc.StartInfo = process;
                proc.Start();
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                if (Timeout > 0)
                {
                    proc.WaitForExit(Timeout);
                }
                else
                {
                    proc.WaitForExit();
                }

                if (proc.HasExited == false)
                    if (proc.Responding)
                        proc.CloseMainWindow();
                    else
                        proc.Kill();

                result.Error = proc.StandardError.ReadToEnd();
                result.Output = proc.StandardOutput.ReadToEnd();

                proc.Close();
            }
            catch (Exception ex)
            {
                result.ExecutionError = ex.Message;
            }

            return result;
        }
    }
}