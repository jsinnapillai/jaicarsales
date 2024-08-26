import { auth } from "@/auth";
import { headers } from "next/headers";

const baseUrl = "http://localhost:6001/";

// Handling Get
async function get(url: string) {
  const requestOptions = {
    method: "GET",
    headers: await getHeaders(),
  };
  const response = await fetch(baseUrl + url, requestOptions);

  return handeresponse(response);
}

// Handling POSt
async function post(url: string, body:{}) {
  const requestOptions = {
    method: "POST",
    headers: await getHeaders(),
    body:JSON.stringify(body)

  };
  const response = await fetch(baseUrl + url, requestOptions);

  return handeresponse(response);
}


// Handling PUT
async function put(url: string, body:{}) {
  const requestOptions = {
    method: "PUT",
    headers: await getHeaders(),
    body:JSON.stringify(body)

  };
  const response = await fetch(baseUrl + url, requestOptions);

  return handeresponse(response);
}


// Handling Delete
async function del(url: string ) {
  const requestOptions = {
    method: "DELETE",
    headers: await getHeaders(),
  };
  const response = await fetch(baseUrl + url, requestOptions);

  return handeresponse(response);
}

// Handling Headers
async function getHeaders(){
  const session = await auth();
  const headers = {
    'Content-type': 'application/json'
  } as any
    if(session?.accessToken)
    {
      headers.Authorization = "Bearer " +session.accessToken
    }

    return headers
  
}

// Handling Response and error

async function handeresponse(response: Response) {
 
  const text = await response.text();
  let data;
  try {
      data = JSON.parse(text);
  } catch (error) {
      data = text;
  }

  if (response.ok) {
      return data || response.statusText;
  } else {
      const error = {
          status: response.status,
          message: response.statusText
      }
      return {error}
  }

}


export const fetchWrapper =  {
        get,
        post,
        put,
        del
} 
