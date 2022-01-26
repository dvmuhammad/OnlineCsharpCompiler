#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#SDK has compiler, unlike runtime (dotnet/aspnet:6.0)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Building isolate
FROM gcc:latest AS isolatebuild
RUN curl -L https://github.com/ioi/isolate/archive/refs/tags/v1.8.1.tar.gz > isolate.tar.gz
RUN tar -xzf isolate.tar.gz
RUN apt-get update
RUN apt-get install libcap-dev -y
RUN apt-get install asciidoc -y
RUN make -C isolate-1.8.1/
RUN mkdir /isolate
RUN cp isolate-1.8.1/*.cf /isolate
RUN cp isolate-1.8.1/LICENSE /isolate/LICENSE-isolate
RUN cp isolate-1.8.1/isolate /isolate/isolate

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OnlineCompiler/Server/OnlineCompiler.Server.csproj", "OnlineCompiler/Server/"]
COPY ["OnlineCompiler/Shared/OnlineCompiler.Shared.csproj", "OnlineCompiler/Shared/"]
COPY ["OnlineCompiler/Client/OnlineCompiler.Client.csproj", "OnlineCompiler/Client/"]
RUN dotnet restore "OnlineCompiler/Server/OnlineCompiler.Server.csproj"
COPY . .
WORKDIR "/src/OnlineCompiler/Server"
RUN dotnet build "OnlineCompiler.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineCompiler.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=isolatebuild /isolate /isolate
RUN apt-get update
RUN apt-get install libcap-dev -y
RUN mv /isolate/default.cf /usr/local/etc/isolate
RUN ln -s /isolate/isolate isolate
ENTRYPOINT ["dotnet", "OnlineCompiler.Server.dll"]
