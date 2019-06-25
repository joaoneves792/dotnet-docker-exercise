#!/bin/sh
# Check for $CLIENT_URLS
if [ -z ${LISTEN_URLS+x} ]; then
        LISTEN_URLS="http://0.0.0.0:2379"
        echo "Using default LISTEN_URLS ($LISTEN_URLS)"
else
        echo "Detected new LISTEN_URLS value of $LISTEN_URLS"
fi

# Check for $PEER_URLS
if [ -z ${ADVERTISE_URLS+x} ]; then
        ADVERTISE_URLS="http://0.0.0.0:2380"
        echo "Using default ADVERTISE_URLS ($ADVERTISE_URLS)"
else
        echo "Detected new ADVERTISE_URLS value of $ADVERTISE_URLS"
fi


/bin/etcd -data-dir=/data --name etcd --advertise-client-urls=${ADVERTISE_URLS} -listen-client-urls=${LISTEN_URLS} $* &
pid=$!

jsonAsPath(){
	jq -r '. as $root | path(..) | . as $path | $root | getpath($path) as $value | select($value | scalars) | ([$path[]] | join("/")) + (" " + $value)' $1
}

env=/etc/vision/$ENVIRONMENT.json
jsonAsPath $env | while read input; do
	until etcdctl put $input; do
		sleep 1
	done
done

wait $pid
