arr=($(find . -name "*.cs"))

#save = "./listing.txt"

echo test
echo $arr

rm ./listing.txt
touch ./listing.txt

for ((i=0; i<${#arr[@]}; i++)); do
#    do something to each element of array
	
	echo $i
	
	filename=${arr[$i]}
	
	num=$((i + 1))
	
	echo "$num. $filename" >> ./listing.txt
	
	cat $filename >> ./listing.txt
	
	echo "" >> ./listing.txt	
	
    echo "${arr[$i]}"
done