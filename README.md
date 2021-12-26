# docker-wsl-wrapper

This command wraps Docker in WSL in a simple command (as a replacement for Docker Desktop). It is written in F#.

## Docker in WSL

### Installation

See https://docs.docker.com/engine/install/ubuntu for information on installing Docker in Ubuntu (WSL).

### Starting daemon

The Docker daemon must be started manually in WSL (Ubuntu) using the following command: `sudo service docker start`

### Docker Compose support

Docker is integrating Compose into the `docker` command. This must be installed manually on Linux. See https://docs.docker.com/compose/cli-command/#install-on-linux for more information.

## Installation

Download the execute from GitHub releases and place it on the path.

## Configuration

The following environment variables can be set to configure the wrapper:

| Variable | Default | Description |
|---|---|---|
| DWW_DOCKER_COMMAND | docker | The Linux Docker command to use |
| DWW_WSL_COMMAND | C:\Windows\System32\wsl.exe | The location of `wsl.exe` in Windows |
| DWW_WSL_DISTRO | Ubuntu-20.04 | The WSL distro to use (see `wsl -l`) |
| DWW_WSL_EXEC | -- | If a default shell is used (see `wsl -?` possible values: `--` or `--exec`) |

## Argument parsing

Arguments are parsed before being passed on to Docker in WSL:

- If `--format` is specified then no version info is outputted
- If a path is specified then the path is transformed to a WSL path using `wslpath` (for example `C:\temp` -> `$(wslpath -u C:/temp)`)

## Debug logging

To enable debug logging: Set the environment variable `DWW_DEBUG` to `true`.
