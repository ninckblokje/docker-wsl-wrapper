(*
  Copyright (c) 2021, ninckblokje
  All rights reserved.
  
  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are met:
  
  * Redistributions of source code must retain the above copyright notice, this
    list of conditions and the following disclaimer.
  
  * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
  
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*)

module DockerWslWrapper =
    open System
    open System.Diagnostics
    open System.IO

    type DockerWslWrapperConfig =
        {
            DockerCommand: string;
            WslCommand: string;
            WslDistro: string;
            WslExec: string;
        }

    type WrappedWslCommand = 
        {
            Command: string;
            Arguments: string list;
            WorkingDirectory: string;
        }
    
    let executeProcess (proc: Process) =
        proc.Start() |> ignore
        proc.WaitForExit -1 |> ignore
        
        proc.ExitCode
    
    let getCommand config args =
        {
            Command = config.WslCommand
            Arguments = ["-d"; config.WslDistro; config.WslExec; config.DockerCommand] |> List.append <| (args |> Array.toList);
            WorkingDirectory = Directory.GetCurrentDirectory();
        }

    let getConfigValue key defaultValue =
        let envValue = System.Environment.GetEnvironmentVariable key
        if envValue = null then defaultValue
        else envValue

    let getConfig =
        {
            DockerCommand = getConfigValue "DWW_DOCKER_COMMAND" "docker";
            WslCommand = getConfigValue "DWW_WSL_COMMAND" "C:\\Windows\\System32\\wsl.exe";
            WslDistro = getConfigValue "DWW_WSL_DISTRO" "Ubuntu-20.04";
            WslExec = getConfigValue "DWW_WSL_EXEC" "--";
        }

    let getProcess cmd =
        let procStartInfo = new ProcessStartInfo(cmd.Command, cmd.Arguments[0..] |> String.concat " ")
        procStartInfo.WorkingDirectory <- cmd.WorkingDirectory

        let proc = new Process()
        proc.StartInfo <- procStartInfo

        proc

open DockerWslWrapper
open System.Diagnostics

[<EntryPoint>]
let main args =
    printfn "docker-wsl-wrapper version %s" <| FileVersionInfo.GetVersionInfo(System.Environment.ProcessPath).ProductVersion;
    printfn "See: https://github.com/ninckblokje/docker-wsl-wrapper"

    getConfig
    |> fun config -> getCommand config args
    |> getProcess
    |> executeProcess
