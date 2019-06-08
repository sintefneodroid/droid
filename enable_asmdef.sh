find . -type f -name '*.asmdef.bak' -exec sh -c 'mv "$0" "${0%.bak}"' {} \;


# Only single folder
#for i in *.bak
#do
#   mv -- "$i" "${i%.bak}"
#done