import azure.functions as func
import datetime
import json
import logging

app = func.FunctionApp()

@app.route(route="HttpExample", auth_level=func.AuthLevel.ANONYMOUS)
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    name = req.params.get('name')
    if not name:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('name')

    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )


@app.queue_trigger(arg_name="azqueue", queue_name="jobs", connection="AzureWebJobsStorage") 
def QueueExample(azqueue: func.QueueMessage):
    print('Queue trigger function processed a queue item: %s', azqueue.get_body().decode('utf-8'))
    logging.info('Python Queue trigger processed a message: %s',
                azqueue.get_body().decode('utf-8'))
