import socket
import threading
import queue
import time
import random

outgoingQ = queue.Queue()


class gamespawn(threading.Thread):
    def __init__(self, msgq, clientlist):
        super(gamespawn, self).__init__()

        self.size = 1024
        self.startt = time.time()
        self.msgq = msgq
        self.nospawnt = [0,0,0]
        self.tempt = 0
        self.b = 4  # need better name
        self.m = 6  # need better name
        self.h = 10
        self.clientlist = clientlist
        # event vars
        self.turns = 0
        self.eventactive = False
        self.meteorx = 7.5
        self.meteory = 3.5
        self.turntimer = 0
        self.temp = 10
        self.killvar = False


    def run(self):
        while (True):  # time.time() => seconds
            try:
                if self.killvar:
                    return False
                # print(type(time.time()))
                current_time = time.time() - self.startt
                # print(time.time())
                # if (self.nospawnt > 0):
                #    #print(self.nospawnt)
                #    self.nospawnt -= time.time() - self.tempt  # need work
                #    self.tempt = time.time()
                if (int(time.time() - self.startt) % self.b == 0) and (current_time - self.nospawnt[0] >= 1):
                    self.nospawnt[0] = current_time
                    for i in range(2):
                        x = round(random.uniform(-7.5, 7.5), 2)  # fix to double
                        y = round(random.uniform(-4, 4), 2)  # fix to double
                        # self.msgq.put('_b(' + str(x) + ', ' + str(y) + ', 0)')
                        # self.msgq.put('_b(' + str(x) + ', ' + str(-y) + ', 0)')
                        posarray = ['_ob(' + str(x) + ', ' + str(y) + ', 0)', '_ob(' + str(x) + ', ' + str(-y) + ', 0)']
                        self.msgq.put(posarray)
                    #self.b = self.b * 3
                if int(time.time() - self.startt) % self.m == 0 and (current_time - self.nospawnt[1] >= 1):
                    self.nospawnt[1] = current_time
                    x = round(random.uniform(-7.5, 7.5), 2)  # fix to double
                    y = round(random.uniform(-4, 4), 2)  # fix to double
                    posarray = ['_om(' + str(x) + ', ' + str(y) + ', 0)', '_om(' + str(x) + ', ' + str(-y) + ', 0)']
                    # self.msgq.put('_b(' + str(x) + ', ' + str(y) + ', 0)')
                    # self.msgq.put('_b(' + str(x) + ', ' + str(-y) + ', 0)')
                    self.msgq.put(posarray)
                    #self.m = self.m * 3
                # new changes
                if int(time.time() - self.startt) % self.h == 0 and (current_time - self.nospawnt[2] >= 1):
                    self.nospawnt[2] = current_time
                    x = round(random.uniform(-7.5, 7.5), 2)  # fix to double
                    y = round(random.uniform(-4, 4), 2)  # fix to double
                    posarray = ['_oh(' + str(x) + ', ' + str(y) + ', 0)', '_oh(' + str(x) + ', ' + str(-y) + ', 0)']
                    # self.msgq.put('_b(' + str(x) + ', ' + str(y) + ', 0)')
                    # self.msgq.put('_b(' + str(x) + ', ' + str(-y) + ', 0)')
                    self.msgq.put(posarray)
                if self.eventactive == False and current_time > self.temp and (current_time - self.turntimer >= 3):
                    self.temp *= 3
                    self.turns = 5
                    self.turntimer = current_time
                    self.eventactive = True
                    speed = [random.randrange(1, 6), random.randrange(1, 6)]
                    y = [round(random.uniform(-self.meteory, self.meteory), 2),
                         round(random.uniform(-self.meteory, self.meteory), 2)]
                    posarray = ['_er' + str(speed[0]) + '(' + str(self.meteorx) + ', ' + str(y[0]) + ', 0)',
                                '_er' + str(speed[0]) + '(' + str(self.meteorx) + ', ' + str(-y[0]) + ', 0)']
                    self.msgq.put(posarray)
                    posarray = ['_el' + str(speed[1]) + '(' + str(-self.meteorx) + ', ' + str(y[1]) + ', 0)',
                                '_el' + str(speed[1]) + '(' + str(-self.meteorx) + ', ' + str(-y[1]) + ', 0)']
                    self.msgq.put(posarray)
                if self.eventactive == True and (current_time - self.turntimer >= 3):
                    self.turns -= 1
                    if self.turns <= 0:
                        self.turns = 0
                        self.eventactive = False
                    self.turntimer = current_time
                    speed = [random.randrange(1, 6), random.randrange(1, 6)]
                    y = [round(random.uniform(-self.meteory, self.meteory), 2),
                         round(random.uniform(-self.meteory, self.meteory), 2)]
                    posarray = ['_er'+str(speed[0])+'(' + str(self.meteorx) + ', ' + str(y[0]) + ', 0)',
                                '_er'+str(speed[0])+'(' + str(self.meteorx) + ', ' + str(-y[0]) + ', 0)']
                    self.msgq.put(posarray)
                    posarray = ['_el'+str(speed[1])+'(' + str(-self.meteorx) + ', ' + str(y[1]) + ', 0)',
                                '_el'+str(speed[1])+'(' + str(-self.meteorx) + ', ' + str(-y[1]) + ', 0)']
                    self.msgq.put(posarray)
                # new changes
                time.sleep(0.00001)
            except Exception as err:
                #print('we ded')
                print(err.args)

                for c in self.clientlist:
                    c.close()
                    c.close()
                # self.parentp.exit()
                return False


class ThreadedServer(threading.Thread):
    def __init__(self, host, port, q):
        super(ThreadedServer, self).__init__()
        self.plist = []
        self.host = host
        self.port = port
        self.q = q
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.sock.bind((self.host, self.port))

        self.pqueue = queue.Queue()
        self.sock.listen(5)
        p = sendToClient(self.q)
        p.start()
        self.plist.append(p)

    def run(self):
        while True:
            if not self.pqueue.empty():
                self.plist.append(self.pqueue.get())
            for p in self.plist:
                print(p.is_alive())
            clientlist = []
            addresslist = []
            for _ in range(2):
                client, address = self.sock.accept()
                print('accepted')
                clientlist.append(client)
                addresslist.append(address)
                # response = str(_+1)
                # msgToSend = [client, response.encode()]
                # self.q.put(msgToSend)
                time.sleep(0.00001)

            p = game(clientlist, addresslist, self.pqueue, self.q)  # check if clients are closed
            p.start()
            # kl = 0
            # gsp = gamespawner(clientlist, addresslist, self.plist, self.q, kl)
            # gsp.start()
            self.plist.append(p)
            print("game " + str(len(self.plist)))
            time.sleep(0.00001)


class game(threading.Thread):
    def __init__(self, clientlist, addresslist, pqueue, q):
        super(game, self).__init__()
        self.clientlist = clientlist
        self.client1 = clientlist[0]
        self.client2 = clientlist[1]
        self.address1 = addresslist[0]
        self.address2 = addresslist[1]
        self.pqueue = pqueue
        self.size = 1024
        # self.msgq = queue.Queue()
        self.q = q

    def run(self):
        print('running game', str(self.address1), str(self.address2))
        response = 'go'
        msgToSend = [self.client1, response.encode()]
        # self.client1.send(response.encode()) send
        self.q.put(msgToSend)
        # msgToSend[0].send(msgToSend[1]) #send right now
        print('sent : ' + str(msgToSend[1].decode('utf8')) + ' to : ' + str(msgToSend[0]))
        data1 = self.client1.recv(self.size)
        print(data1)
        # self.q.put(msgToSend)
        msgToSend = [self.client2, response.encode()]
        # self.q.put(msgToSend)
        # msgToSend[0].send(msgToSend[1]) #send right now
        # self.client2.send(response.encode()) send
        self.q.put(msgToSend)
        print('sent : ' + str(msgToSend[1].decode('utf8')) + ' to : ' + str(msgToSend[0]))
        data2 = self.client2.recv(self.size)
        print(data2)
        print(data1.decode('utf8') + ' ' + data2.decode('utf8'))
        msgq = queue.Queue()
        gsp = gamespawn(msgq, self.clientlist)
        # print(type(gsp))
        # self.plist.append(gsp)
        gsp.start()
        self.pqueue.put(gsp)
        #gsp.join()
        check = True
        if data1.decode('utf8') == response and data2.decode('utf8') == response:
            while check:
                try:
                    data1 = self.client1.recv(self.size)
                    # print(data1)
                    data2 = self.client2.recv(self.size)
                    # print(data2)
                    dat1temp = data1.decode('utf8')
                    dat2temp = data2.decode('utf8')
                    if dat1temp == 'error' or dat2temp == 'error' or dat1temp == '' or dat2temp == '':
                        print(data1.decode('utf8'))
                        print(data2.decode('utf8'))
                        raise Exception('Error in one of the clients.')
                    # self.client1.send(data1) # protocol
                    # print('sent : ' + str(data1.decode('utf8')) + ' to : ' + str(self.client1)) # protocol
                    # data1 = data1.decode('utf8')

                    # data2 = self.client2.recv(self.size)
                    # print(data2)
                    # self.client2.send(data2) # protocol
                    # print('sent : ' + str(data2.decode('utf8')) + ' to : ' + str(self.client2)) # protocol
                    # data2 = data2.decode('utf8')
                    # msgToSend[0].send(msgToSend[1])
                    while not (msgq.empty()):
                        pos = msgq.get()
                        data1 = (data1.decode('utf8') + pos[0]).encode()
                        data2 = (data2.decode('utf8') + pos[1]).encode()

                    msgToSend = [self.client2, data1]
                    self.q.put(msgToSend)
                    # self.client2.send(data1) send
                    # print('sent : ' + str(msgToSend[1].decode('utf8')) + ' to : ' + str(msgToSend[0]))
                    # if (msgToSend[0].recv(self.size) != msgToSend[1]): # protocol
                    #    raise ValueError('A very specific bad thing happened. in client 2') # protocol
                    msgToSend = [self.client1, data2]
                    # msgToSend[0].send(msgToSend[1])
                    # self.client1.send(data2) send
                    self.q.put(msgToSend)
                    # print('sent : ' + str(msgToSend[1].decode('utf8')) + ' to : ' + str(msgToSend[0]))
                    # if(msgToSend[0].recv(self.size) != msgToSend[1]): # protocol
                    #    raise ValueError('A very specific bad thing happened. in client 1') # protocol
                    # self.q.put(msgToSend)
                    if("won" ==dat1temp or "lost" ==dat1temp or "won" ==dat2temp or "lost" ==dat2temp):
                        raise Exception("game ended successfully")
                    time.sleep(0.00001)


                except Exception as e:
                    print(e.args)

                    self.client1.close()
                    self.client2.close()

                    #gsp.exit()
                    gsp.killvar = True
                    check = False
                    #break


class sendToClient(threading.Thread):
    def __init__(self, q):
        super(sendToClient, self).__init__()

        self.size = 1024
        self.q = q

    def run(self):
        print('start send process')
        while True:
            if not self.q.empty():
                try:
                    message = self.q.get()
                    print(message[1].decode('utf8'))
                    message[0].send(message[1])
                except:
                    pass
            time.sleep(0.00001)


if __name__ == "__main__":
    """while True:
        port_num = input("Port? ")
        try:
            port_num = int(port_num)
            break
        except ValueError:
            pass"""
    port_num = int(8000)
    Ts = ThreadedServer('0.0.0.0', port_num, outgoingQ)
    Ts.start()
    Ts.join()
