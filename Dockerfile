# base image, has only what is needed to execute SocialNexusClient.dll
FROM	mcr.microsoft.com/dotnet/aspnet:8.0	AS	base
WORKDIR	/app

# build image, has what is needed to create SocialNexusClient.dll
FROM	mcr.microsoft.com/dotnet/sdk:8.0	AS	build
WORKDIR	/src
COPY	SocialNexusClient.csproj	.
RUN	dotnet restore "SocialNexusClient.csproj"
COPY	.	.
RUN	dotnet build "SocialNexusClient.csproj" -c Release -o /app/build

# piggy back off of build image, publish SocialNexusClient
FROM build as publish
RUN dotnet publish "SocialNexusClient.csproj" -c Release -o /app/publish
WORKDIR /app/publish

# take the complete SocialNexusClient and dependencies only as final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# tell docker what command to run when started, dotnet SocialNexusClient.dll
ENTRYPOINT	["dotnet","SocialNexusClient.dll"]
