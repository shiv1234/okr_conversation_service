# ASP.NET Core Alpine 

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
ARG ENVNAME=RESOURCEGROUOP
ARG License=xxx
RUN echo ${ENVNAME}
RUN if [ "$ENVNAME" = "unlockokr-prod" ] ; then License=c60db198bb8e6047299e007f6aa051c0c82cNRAL ; else License=xxx ; fi
RUN echo ${License}
# Move source files to /src folder
COPY . /src

WORKDIR /src
# Build the release executable
RUN dotnet publish "OkrConversationService.Application/OkrConversationService.Application.csproj" -c Release -o /out

# Release image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add icu-libs




ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=${License} \
NEW_RELIC_APP_NAME=${ENVNAME}

ARG AGENT_PACKAGE_URL=https://download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent_10.10.0_amd64.tar.gz

RUN cd "/usr/local/" && \
  wget -q ${AGENT_PACKAGE_URL} -O - | tar xzf - && \
  cd ${CORECLR_NEWRELIC_HOME} && \
  chmod +x run.sh



# Copy binaries from build-env /out to alpine's folder (/app)
COPY --from=build-env /out /app

WORKDIR /app
ENTRYPOINT sh ${CORECLR_NEWRELIC_HOME}/run.sh dotnet OkrConversationService.Application.dll

