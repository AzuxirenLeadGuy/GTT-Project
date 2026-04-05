# .devcontainer/Dockerfile
FROM localhost/dotnetpod AS base

RUN dotnet new install MonoGame.Templates.CSharp

# ---- Set workdir and switch user ----
WORKDIR /workspace
USER root

