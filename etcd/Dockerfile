FROM        alpine:edge
ENV         ETCDCTL_API 3
VOLUME      /data
EXPOSE      2379 2380
RUN         apk add --update \
                ca-certificates \
                openssl \
                tar \ 
                jq && \
            wget https://github.com/coreos/etcd/releases/download/v3.3.13/etcd-v3.3.13-linux-amd64.tar.gz && \
            tar xzvf etcd-v3.3.13-linux-amd64.tar.gz && \
            mv etcd-v3.3.13-linux-amd64/etcd* /bin/ && \
            apk del --purge tar openssl && \
            rm -Rf etcd-v3.3.13-linux-amd64* /var/cache/apk/*
ADD         run.sh /bin/run.sh
COPY        *.json /etc/vision/
ENTRYPOINT  ["/bin/run.sh"]

