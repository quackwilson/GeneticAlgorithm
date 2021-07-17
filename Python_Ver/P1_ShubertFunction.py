import math
import random

#generate Population
def generatePopulation(n_gene,n_chro):
    chro=[]
    for i in range (0,n_chro):
        gene=[]
        for j in range (0,n_gene):
            n=random.randint(0,1)
            gene.append(str(n))
        chro.append("".join(gene))
    return chro

#decoder (input:str;output:str)
def binaryToDecimal(binary):
    point = binary.find('.')
    if (point == -1) :
        point = len(binary)
    intDecimal = 0
    twos = 1
    for i in range(point-1, -1, -1) :
        intDecimal += ((ord(binary[i])-ord('0')) * twos)
        twos *= 2
    fracDecimal = 0
    twos = 2
    for i in range(point + 1, len(binary)):
        fracDecimal += ((ord(binary[i])-ord('0')) / twos)
        twos *= 2.0
    return intDecimal + fracDecimal

def decoder(chro):
    chro_int=[]
    for i in range (1,int(len(chro)/2)+1):
        chro_int.append(chro[i])
    chro_int.append(".")
    for i in range (int(len(chro)/2)+1,len(chro)):
        chro_int.append(chro[i])
    chro_int="".join(chro_int)
    if chro[0]=="0":
        return "-"+str(binaryToDecimal(chro_int))
    else:
        return str(binaryToDecimal(chro_int))

#scaling
def scaling(value,upper,lower,n_gene):
    max_bin=[]
    for i in range (0,n_gene):
        max_bin.append("1")
    max_bin="".join(max_bin)
    max_bin=float(decoder(max_bin))
    m=upper-lower
    n=value-max_bin
    return m*(n/(2*max_bin))+upper

#Shubert Function
upper=10
lower=-10

def shubertFunction(x1,x2):
    a=0
    b=0
    for i in range(0,5):
        a+=i*math.cos(((i+1)*x1)+i)
        b+=i*math.cos(((i+1)*x2)+i)
    return a*b

##to find minimum
def fitness(y):
    return 0-y

#select chromosome by fitness
def selectedLocation(fitness):
    range0=[0]
    for i in range (0,len(fitness)):
        range0.append((range0[i]+fitness[i])*100)
    locationSelected=[0,0]
    ran1=random.randint(1,100)
    for i in range (0,len(fitness)):
        if ran1>range0[i] and ran1<=range0[i+1]:
            locationSelected[0]=i
            break
    ran2=random.randint(1,100)
    for i in range (0,len(fitness)):
        if ran2>range0[i] and ran2<=range0[i+1]:
            locationSelected[1]=i
    if locationSelected[0]==locationSelected[1]:
        locationSelected[1]=locationSelected[1]+1
    return locationSelected

#Genetic Algorithm Operator

##mutation
def mutation(oldX,fitness):
    locationChro=selectedLocation(fitness)[0]
    ran=random.randint(0,len(oldX[0])-1)
    chroMutation=list(oldX[locationChro])
    if chroMutation[ran]=="1":
        chroMutation[ran]="0"
    elif chroMutation[ran]=="0":
        chroMutation[ran]="1"
    newX=[]
    newX.append("".join(chroMutation))
    return newX

##crossover
def crossover(oldX,fitness):
    locations=[]
    locations.append(selectedLocation(fitness))
    chrosCrossover=[list(oldX[locations[0]]),list(oldX[locations[1]])]
    ran=random.randint(4,len(oldX[0])-4)
    newX=[]
    for i in range (0,ran):
        newX.append(chrosCrossover[0][i])
    for i in range (ran,len(oldX[0])):
        newX.append(chrosCrossover[1][i])
    newX="".join(newX)
    return newX

##reproduction
def reproduction(oldX,fitness):
    return oldX[selectedLocation(fitness)[0]]

#GA operator selector
def NewPopulation(oldX1,oldX2,fitness):
    newX1=[]
    newX2=[]
    mutationRate,crossoverRate,reproductionRate=20,70,10
    for i in range (0,len(oldX1)):
        ran=random.randint(1,100)
        if ran>0 & ran<= mutationRate:
            newX1.append(mutation(oldX1,fitness))
        elif ran>mutationRate & ran<=mutationRate+crossoverRate:
            newX1.append(crossover(oldX1,fitness))
        elif ran>mutationRate+crossoverRate & ran<=mutationRate+crossoverRate+reproductionRate:
            newX1.append(reproduction(oldX1,fitness))
        ran=random.randint(1,100)
        if ran>0 & ran<= mutationRate:
            newX2.append(mutation(oldX2,fitness))
        elif ran>mutationRate & ran<=mutationRate+crossoverRate:
            newX2.append(crossover(oldX2,fitness))
        elif ran>mutationRate+crossoverRate & ran<=mutationRate+crossoverRate+reproductionRate:
            newX2.append(reproduction(oldX2,fitness))
    NextPopulations=[newX1,newX2]
    return NextPopulations

#main function
n_chro=100
n_gene=21
iterationTimes=500
newPopulation1,newPopulation2=generatePopulation(n_gene,n_chro),generatePopulation(n_gene,n_chro)
x1,x2,y=([]for i in range(3))
yFitness=[]
for i in range (0,n_chro):
    x1.append(scaling(float(decoder(newPopulation1[i])),upper,lower,n_gene))
    x2.append(scaling(float(decoder(newPopulation2[i])),upper,lower,n_gene))
    y.append(shubertFunction(x1[i],x2[i]))
    yFitness.append(fitness(y[i]))
bestFitness=max(yFitness)
bestIndex=yFitness.index(max(yFitness))
bestX1s=newPopulation1[bestIndex]
bestX1=scaling(float(decoder(newPopulation1[bestIndex])),upper,lower,n_gene)
bestX2s=newPopulation2[bestIndex]
bestX2=scaling(float(decoder(newPopulation2[bestIndex])),upper,lower,n_gene)
bestY=shubertFunction(bestX1,bestX2)
print(bestX1,bestX2,bestY)
for i in range (0,iterationTimes):
    Next=NewPopulation(newPopulation1,newPopulation2,yFitness)
    for j in range (0,n_chro):
        newPopulation1[j]=Next[0][j]
        newPopulation2[j]=Next[1][j]
    testFitness=[]
    for j in range (0,n_chro):
        x1[j]=scaling(float(decoder(newPopulation1[j])),upper,lower,n_gene)
        x2[j]=scaling(float(decoder(newPopulation2[j])),upper,lower,n_gene)
        y[j]=shubertFunction(x1[j],x2[j])
        testFitness.append(fitness(y[j]))
    worstFitness=min(testFitness)
    worstIndex=testFitness.index(min(testFitness))
    newPopulation1[worstIndex]=bestX1s
    newPopulation2[worstIndex]=bestX2s
    for j in range (0,n_chro):
        x1[j]=scaling(float(decoder(newPopulation1[j])),upper,lower,n_gene)
        x2[j]=scaling(float(decoder(newPopulation2[j])),upper,lower,n_gene)
        y[j]=shubertFunction(x1[j],x2[j])
        yFitness[j]=fitness(y[j])
    bestFitness=max(yFitness)
    bestIndex=yFitness.index(max(yFitness))
    bestX1s=newPopulation1[bestIndex]
    bestX1=scaling(float(decoder(newPopulation1[bestIndex])),upper,lower,n_gene)
    bestX2s=newPopulation2[bestIndex]
    bestX2=scaling(float(decoder(newPopulation2[bestIndex])),upper,lower,n_gene)
    bestY=shubertFunction(bestX1,bestX2)
    print(bestX1,bestX2,bestY)