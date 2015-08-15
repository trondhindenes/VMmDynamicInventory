#!/usr/bin/env python

import requests
import sys, getopt
from ansible.constants import p, get_config
from ansible import utils

operation = str(sys.argv[1])



vmminventory_uri = get_config(p, "vmminventory", "vmminventory_uri", "VMMINVENTORY_URI","")
#print(armrest_uri)
vmminventory_use_cache = get_config(p, "vmminventory", "vmminventory_use_cache", "VMMINVENTORY_USE_CACHE","")
vmminventory_cache_lifetime_seconds = get_config(p, "vmminventory", "vmminventory_cache_lifetime_seconds", "VMMINVENTORY_CACHE_LIFETIME_SECONDS","")

if (vmminventory_use_cache == True):
	import requests_cache
	requests.cache.install_cache('vmminventory_cache', backend='sqlite', expire_after=vmminventory_use_cache)

if (operation == '--list'):
	vmminventory_list_uri = vmminventory_uri + "/api/listhosts"
	r = requests.get(vmminventory_list_uri)
	print(r.text)