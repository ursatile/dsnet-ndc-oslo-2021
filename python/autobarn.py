import grpc
import price_pb2_grpc as pb2_grpc
import price_pb2 as pb2
from concurrent import futures
import time
import os

class PricingService(pb2_grpc.PricerServicer):
    def __init__(self, *args, **kwargs):
        pass

    def GetPrice(self, request, context):
        print(request)
        result = {'price': 12345, 'currencyCode': 'RUB'}
        return pb2.PriceReply(**result)

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    pb2_grpc.add_PricerServicer_to_server(PricingService(), server)
    server.add_insecure_port('[::]:5002')
    key = open('d:/dropbox/workshop.ursatile.com/workshop.ursatile.com.key', 'rb').read()
    crt = open('d:/dropbox/workshop.ursatile.com/workshop.ursatile.com.crt', 'rb').read()
    server_credentials = grpc.ssl_server_credentials(((key, crt,),))
    server.add_secure_port('[::]:5003', server_credentials)
    server.start()
    print("Autobarn gRPC Pricing Server running.")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()