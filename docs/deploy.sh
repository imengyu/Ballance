#!/usr/bin/env bash
basedir=`cd $(dirname $0); pwd -P`
scp -r ./* root@imengyu.top:/home/www/ballance-docs/