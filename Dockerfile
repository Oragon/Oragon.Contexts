FROM microsoft/dotnet:2.1-sdk

RUN export PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console --version 1.4.1
#RUN chmod +x /root/.dotnet/tools/.store/dotnet-sonarscanner/4.3.1/dotnet-sonarscanner/4.3.1/tools/netcoreapp2.1/any/sonar-scanner-3.2.0.1227/bin/sonar-scanner
#RUN cat /root/.dotnet/tools/.store/dotnet-sonarscanner/4.3.1/dotnet-sonarscanner/4.3.1/tools/netcoreapp2.1/any/sonar-scanner-3.2.0.1227/bin/sonar-scanner


RUN apt-get update && apt-get install -y --no-install-recommends \
		bzip2 \
		unzip \
		xz-utils \
	&& rm -rf /var/lib/apt/lists/*
ENV LANG C.UTF-8

RUN { \
		echo '#!/bin/sh'; \
		echo 'set -e'; \
		echo; \
		echo 'dirname "$(dirname "$(readlink -f "$(which javac || which java)")")"'; \
	} > /usr/local/bin/docker-java-home \
	&& chmod +x /usr/local/bin/docker-java-home

RUN ln -svT "/usr/lib/jvm/java-8-openjdk-$(dpkg --print-architecture)" /docker-java-home
ENV JAVA_HOME /docker-java-home/jre
ENV JAVA_VERSION 8u151
ENV JAVA_DEBIAN_VERSION 8u151-b12-1~deb9u1
ENV CA_CERTIFICATES_JAVA_VERSION 20170531+nmu1
RUN set -ex; \
	\
	if [ ! -d /usr/share/man/man1 ]; then \
		mkdir -p /usr/share/man/man1; \
	fi; \
	\
	apt-get update; \
	apt-get install -y \
		openjdk-8-jre-headless \
		ca-certificates-java \
	; \
	rm -rf /var/lib/apt/lists/*; \
	\
	[ "$(readlink -f "$JAVA_HOME")" = "$(docker-java-home)" ]; \
	\
	update-alternatives --get-selections | awk -v home="$(readlink -f "$JAVA_HOME")" 'index($3, home) == 1 { $2 = "manual"; print | "update-alternatives --set-selections" }'; \
	update-alternatives --query java | grep -q 'Status: manual'

RUN /var/lib/dpkg/info/ca-certificates-java.postinst configure
